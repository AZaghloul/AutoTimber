using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc;
using Xbim.Ifc.ViewModels;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;
using Xbim.Common;
using Bim.Domain.Ifc;

namespace Bim.Domain.Polygon
{
    public class WallPolygon
    {
        #region Properties
        public List<IfOpening> Openings { get; set; }
        public List<Region> Regions { get; set; }
        public List<Region> RLeft
        {
            get
            {
                return Regions.Where(a => a.RegionLocation == RegionLocation.Left).ToList();
            }
        }
        public List<Region> RRight
        {
            get
            {
                return Regions.Where(a => a.RegionLocation == RegionLocation.Right).ToList();
            }
        }
        public List<Region> RTop
        {
            get
            {
                return Regions.Where(a => a.RegionLocation == RegionLocation.Top).ToList();
            }
        }
        public List<Region> RBottom
        {
            get
            {
                return Regions.Where(a => a.RegionLocation == RegionLocation.Bottom).ToList();
            }
        }
        public List<Region> RBetween
        {
            get
            {
                return Regions.Where(a => a.RegionLocation == RegionLocation.Middle).ToList();
            }
        }
        public bool IsOpen { get; set; }
        public IfWall IfWall { get; set; }

        #endregion

        #region Constructor
        public WallPolygon(IfWall ifWall)
        {
            IfWall = ifWall;
            Openings = new List<IfOpening>();
            Regions = new List<Region>();
            Openings = IfWall.Openings;
            GetRegions();
        }

        #endregion

        #region Methods
        public void GetRegions()
        {
            if (Openings.Count == 0)
            {
                IsOpen = false;
            }
            else
            {
                IsOpen = true;
                List<IfOpening> SortedOpens = Openings.OrderBy(open => open.IfLocation.X).ToList();

                if (Openings.Count == 1)
                {
                    Region drr = new Region(
                       Math.Abs( IfWall.IfDimension.XDim - ((Openings[0].IfLocation.X) + Openings[0].IfDimension.XDim)),
                        IfWall.IfDimension.YDim,
                        IfWall.IfDimension.ZDim,
                        Math.Abs((Openings[0].IfLocation.X) + Openings[0].IfDimension.XDim), 0, 0, RegionLocation.Right, Openings[0].Direction);


                    Region drl = new Region(
                        Openings[0].IfLocation.X,
                        IfWall.IfDimension.YDim,
                        IfWall.IfDimension.ZDim, 0, 0, 0, RegionLocation.Left,Openings[0].Direction);

                    Regions.Add(drr);
                    Regions.Add(drl);

                    switch (Openings[0].OpeningType)
                    {

                        case OpeningType.Door:
                            Region drt = new Region(
                                Openings[0].IfDimension.XDim,
                                IfWall.IfDimension.YDim,
                               Math.Abs(IfWall.IfDimension.ZDim - (Openings[0].IfLocation.Z + Openings[0].IfDimension.ZDim)),
                                Openings[0].IfLocation.X,
                                0,
                                Openings[0].IfLocation.Z + Openings[0].IfDimension.ZDim, RegionLocation.Top,Openings[0].Direction);
                            drt.LocalPlacement = Openings[0].LocalPlacement;
                            Regions.Add(drt);

                            break;
                        case OpeningType.Window:
                            Region wrt = new Region(
                                Openings[0].IfDimension.XDim,
                                IfWall.IfDimension.YDim,
                                IfWall.IfDimension.ZDim - (Openings[0].IfLocation.Z + Openings[0].IfDimension.ZDim),
                                Openings[0].IfLocation.X, 0,
                                Openings[0].IfLocation.Z + Openings[0].IfDimension.ZDim, RegionLocation.Top, Openings[0].Direction);
                            wrt.LocalPlacement = Openings[0].LocalPlacement;
                            Region wrb = new Region(
                                Openings[0].IfDimension.XDim,
                                IfWall.IfDimension.YDim,
                                Openings[0].IfLocation.Z,
                                Openings[0].IfLocation.X,
                                0, 0, RegionLocation.Bottom,Openings[0].Direction);
                            wrt.LocalPlacement = Openings[0].LocalPlacement;
                            Regions.Add(wrt);
                            Regions.Add(wrb);
                            break;
                    }
                }
                else
                {
                    var tempOpening = new IfOpening(Openings.Last());
                    var flippedOpenning = new List<IfOpening>();
                    if (tempOpening.Direction==Direction.Negative)
                    {
                        tempOpening.Flip(Axis.xAxis);
                    }
                    Region drr = new Region(
                        Math.Abs(IfWall.IfDimension.XDim - ((tempOpening.IfLocation.X) + tempOpening.IfDimension.XDim)),
                        IfWall.IfDimension.YDim,
                        IfWall.IfDimension.ZDim,
                        (tempOpening.IfLocation.X) + tempOpening.IfDimension.XDim,
                        0, 0, RegionLocation.Right,tempOpening.Direction);

                    Region drl = new Region(
                        Openings[0].IfLocation.X,
                        IfWall.IfDimension.YDim,
                        IfWall.IfDimension.ZDim,
                        0, 0, 0, RegionLocation.Left,tempOpening.Direction);

                    Regions.Add(drr);
                    Regions.Add(drl);

                    for (int i = 0; i < Openings.Count - 1; i++)
                    {
                        

                        foreach (var opening in Openings)
                        {
                            flippedOpenning.Add(new IfOpening(opening));
                        }
                        foreach (var fo in flippedOpenning)
                        {
                            if (fo.Direction==Direction.Negative)
                            {
                                fo.Flip(Axis.xAxis);
                               fo.Direction= Direction.Positive;
                            }
                        }
                        Region rr = new Region();
                        Region rbetween = new Region(
                            Math.Abs(flippedOpenning[i + 1].IfLocation.X - (flippedOpenning[i].IfLocation.X + flippedOpenning[i].IfDimension.XDim)),
                            IfWall.IfDimension.YDim,
                            IfWall.IfDimension.ZDim,
                            flippedOpenning[i].IfLocation.X + flippedOpenning[i].IfDimension.XDim,
                            0, 0, RegionLocation.Middle, flippedOpenning[i].Direction);

                        Regions.Add(rbetween);
                    }
                    for (int i = 0; i < flippedOpenning.Count; i++)
                    {
                        Region rt = new Region(
                            flippedOpenning[i].IfDimension.XDim,
                            IfWall.IfDimension.YDim,
                            IfWall.IfDimension.ZDim - (flippedOpenning[i].IfLocation.Z + flippedOpenning[i].IfDimension.ZDim),
                            flippedOpenning[i].IfLocation.X,
                            0,
                            flippedOpenning[i].IfLocation.Z + flippedOpenning[i].IfDimension.ZDim, RegionLocation.Top, flippedOpenning[i].Direction);
                        rt.LocalPlacement = flippedOpenning[i].LocalPlacement;
                        Regions.Add(rt);
                        if (flippedOpenning[i].OpeningType == OpeningType.Window)
                        {
                            Region rb = new Region(
                               flippedOpenning[i].IfDimension.XDim,
                                IfWall.IfDimension.YDim,
                                flippedOpenning[i].IfLocation.Z,
                                flippedOpenning[i].IfLocation.X,
                                0, 0, RegionLocation.Bottom, flippedOpenning[i].Direction);
                            rb.LocalPlacement = flippedOpenning[i].LocalPlacement;
                            Regions.Add(rb);
                        }
                    }
                }
            }
        }
        #endregion
        #region Static Methods


        public static List<WallPolygon> GetPolygons(List<IfWall> walls)
        {

            List<WallPolygon> res = new List<WallPolygon>();
            foreach (var wall in walls)
            {
                res.Add(new WallPolygon(wall));
            }

            return res;
        }
        #endregion
    }
}


//if (IfWall.Doors != null)
//{
//    if (IfWall.IfLocation.X == IfWall.Doors[0].IfLocation.X & IfWall.IfLocation.Y < IfWall.Doors[0].IfLocation.Y)
//    {
//        Direction = Direction.XconstYpos;
//    }
//    else if (IfWall.IfLocation.X == IfWall.Doors[0].IfLocation.X & IfWall.IfLocation.Y > IfWall.Doors[0].IfLocation.Y)
//    {
//        Direction = Direction.XconstYneg;
//    }
//    else if (IfWall.IfLocation.Y == IfWall.Doors[0].IfLocation.Y & IfWall.IfLocation.X < IfWall.Doors[0].IfLocation.X)
//    {
//        Direction = Direction.XposYconst;
//    }
//    else if (IfWall.IfLocation.Y == IfWall.Doors[0].IfLocation.Y & IfWall.IfLocation.X > IfWall.Doors[0].IfLocation.X)
//    {
//        Direction = Direction.XnegYconst;
//    }
//    else if (IfWall.IfLocation.Y < IfWall.Doors[0].IfLocation.Y & IfWall.IfLocation.X < IfWall.Doors[0].IfLocation.X)
//    {
//        Direction = Direction.XposYpos;
//    }
//    else if (IfWall.IfLocation.Y > IfWall.Doors[0].IfLocation.Y & IfWall.IfLocation.X > IfWall.Doors[0].IfLocation.X)
//    {
//        Direction = Direction.XnegYneg;
//    }
//    else if (IfWall.IfLocation.Y > IfWall.Doors[0].IfLocation.Y & IfWall.IfLocation.X < IfWall.Doors[0].IfLocation.X)
//    {
//        Direction = Direction.XposYneg;
//    }
//    else if (IfWall.IfLocation.Y < IfWall.Doors[0].IfLocation.Y & IfWall.IfLocation.X > IfWall.Doors[0].IfLocation.X)
//    {
//        Direction = Direction.XnegYpos;
//    }
//}
//else if (IfWall.windows != null)
//{
//    if (IfWall.IfLocation.X == IfWall.windows[0].IfLocation.X & IfWall.IfLocation.Y < IfWall.windows[0].IfLocation.Y)
//    {
//        Direction = Direction.XconstYpos;
//    }
//    else if (IfWall.IfLocation.X == IfWall.windows[0].IfLocation.X & IfWall.IfLocation.Y > IfWall.windows[0].IfLocation.Y)
//    {
//        Direction = Direction.XconstYneg;
//    }
//    else if (IfWall.IfLocation.Y == IfWall.windows[0].IfLocation.Y & IfWall.IfLocation.X < IfWall.windows[0].IfLocation.X)
//    {
//        Direction = Direction.XposYconst;
//    }
//    else if (IfWall.IfLocation.Y == IfWall.windows[0].IfLocation.Y & IfWall.IfLocation.X > IfWall.windows[0].IfLocation.X)
//    {
//        Direction = Direction.XnegYconst;
//    }
//    else if (IfWall.IfLocation.Y < IfWall.windows[0].IfLocation.Y & IfWall.IfLocation.X < IfWall.windows[0].IfLocation.X)
//    {
//        Direction = Direction.XposYpos;
//    }
//    else if (IfWall.IfLocation.Y > IfWall.windows[0].IfLocation.Y & IfWall.IfLocation.X > IfWall.windows[0].IfLocation.X)
//    {
//        Direction = Direction.XnegYneg;
//    }
//    else if (IfWall.IfLocation.Y > IfWall.windows[0].IfLocation.Y & IfWall.IfLocation.X < IfWall.windows[0].IfLocation.X)
//    {
//        Direction = Direction.XposYneg;
//    }
//    else if (IfWall.IfLocation.Y < IfWall.windows[0].IfLocation.Y & IfWall.IfLocation.X > IfWall.windows[0].IfLocation.X)
//    {
//        Direction = Direction.XnegYpos;
//    }
//}
