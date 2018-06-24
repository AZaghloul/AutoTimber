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
using Bim.Common.Measures;

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
            Length JoistDepth = Length.FromInches(IfJoist.Setup.Get<RecSection>("RecSection").Depth);
            Length RegionHeight = IfWall.IfDimension.ZDim - JoistDepth - Length.FromInches(2*2);
            IfDimension WallDimension = new IfDimension(IfWall.IfDimension.XDim, IfWall.IfDimension.YDim, RegionHeight);
            if (Openings.Count == 0)
            {
                var reg = new Region()
                {
                    IfDimension = new IfDimension(WallDimension),
                    IfLocation = new IfLocation(),
                    RegionLocation = RegionLocation.Left,
                    Direction = IfWall.Direction
                };
                RLeft.Add(reg);
                Regions.Add(reg);
            }
            else
            {
                IsOpen = true;
                Openings = Openings.OrderBy(open => open.IfLocation.X.Inches).ToList();

                if (Openings.Count == 1)
                {
                    var tempOpening = new IfOpening(Openings[0]);
                    if (tempOpening.Direction == Direction.Negative)
                    {
                        tempOpening.Flip(Axis.xAxis);
                        tempOpening.Direction = Direction.Positive;
                    }

                    Region drr = new Region(
                        Math.Abs(IfWall.IfDimension.XDim - ((tempOpening.IfLocation.X) + tempOpening.IfDimension.XDim)),
                        IfWall.IfDimension.YDim,
                        WallDimension.ZDim,
                        Math.Abs((tempOpening.IfLocation.X) + tempOpening.IfDimension.XDim),
                        0,
                        0,
                        RegionLocation.Right, tempOpening.Direction);


                    Region drl = new Region(
                        tempOpening.IfLocation.X,
                        IfWall.IfDimension.YDim,
                        WallDimension.ZDim,
                        0, 0, 0, RegionLocation.Left, tempOpening.Direction);

                    Regions.Add(drr);
                    Regions.Add(drl);

                    switch (Openings[0].OpeningType)
                    {
                        case OpeningType.Door:
                            Region drt = new Region(
                                tempOpening.IfDimension.XDim,
                                IfWall.IfDimension.YDim,
                                Math.Abs(WallDimension.ZDim - (tempOpening.IfLocation.Z + tempOpening.IfDimension.ZDim)),
                                tempOpening.IfLocation.X,
                                0,
                                tempOpening.IfLocation.Z + tempOpening.IfDimension.ZDim,
                                RegionLocation.Top, tempOpening.Direction);
                            drt.LocalPlacement = tempOpening.LocalPlacement;
                            Regions.Add(drt);

                            break;
                        case OpeningType.Window:
                            Region wrt = new Region(
                                tempOpening.IfDimension.XDim,
                                IfWall.IfDimension.YDim,
                                WallDimension.ZDim - (tempOpening.IfLocation.Z + tempOpening.IfDimension.ZDim),
                                tempOpening.IfLocation.X, 0,
                                tempOpening.IfLocation.Z + tempOpening.IfDimension.ZDim, RegionLocation.Top, tempOpening.Direction);
                            wrt.LocalPlacement = tempOpening.LocalPlacement;
                            Region wrb = new Region(
                                tempOpening.IfDimension.XDim,
                                IfWall.IfDimension.YDim,
                                tempOpening.IfLocation.Z,
                                tempOpening.IfLocation.X,
                                0, 0, RegionLocation.Bottom, tempOpening.Direction);
                            wrt.LocalPlacement = tempOpening.LocalPlacement;
                            Regions.Add(wrt);
                            Regions.Add(wrb);
                            break;
                    }
                }
                else
                {
                    var tempOpening = new IfOpening(Openings.Last());
                    var flippedOpenning = new List<IfOpening>();
                    if (tempOpening.Direction == Direction.Negative)
                    {
                        tempOpening.Flip(Axis.xAxis);
                    }
                    Region drr = new Region(
                        Math.Abs(IfWall.IfDimension.XDim - ((tempOpening.IfLocation.X) + tempOpening.IfDimension.XDim)),
                        IfWall.IfDimension.YDim,
                        WallDimension.ZDim,
                        (tempOpening.IfLocation.X) + tempOpening.IfDimension.XDim,
                        0, 0, RegionLocation.Right, tempOpening.Direction);

                    tempOpening = new IfOpening(Openings[0]);
                    if (tempOpening.Direction == Direction.Negative)
                    {
                        tempOpening.Flip(Axis.xAxis);
                    }
                    Region drl = new Region(
                        tempOpening.IfLocation.X,
                        IfWall.IfDimension.YDim,
                        WallDimension.ZDim,
                        0, 0, 0, RegionLocation.Left, tempOpening.Direction);

                    Regions.Add(drr);
                    Regions.Add(drl);
                    foreach (var opening in Openings)
                    {
                        flippedOpenning.Add(new IfOpening(opening));
                    }
                    foreach (var fo in flippedOpenning)
                    {
                        if (fo.Direction == Direction.Negative)
                        {
                            fo.Flip(Axis.xAxis);
                            fo.Direction = Direction.Positive;
                        }
                    }

                    for (int i = 0; i < Openings.Count - 1; i++)
                    {


                        Region rr = new Region();
                        Region rbetween = new Region(
                            Math.Abs(flippedOpenning[i + 1].IfLocation.X - (flippedOpenning[i].IfLocation.X + flippedOpenning[i].IfDimension.XDim)),
                            IfWall.IfDimension.YDim,
                            WallDimension.ZDim,
                            flippedOpenning[i].IfLocation.X + flippedOpenning[i].IfDimension.XDim,
                            0, 0, RegionLocation.Middle, flippedOpenning[i].Direction);

                        Regions.Add(rbetween);
                    }
                    for (int i = 0; i < flippedOpenning.Count; i++)
                    {
                        Region rt = new Region(
                            flippedOpenning[i].IfDimension.XDim,
                            IfWall.IfDimension.YDim,
                            WallDimension.ZDim - (flippedOpenning[i].IfLocation.Z + flippedOpenning[i].IfDimension.ZDim),
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
