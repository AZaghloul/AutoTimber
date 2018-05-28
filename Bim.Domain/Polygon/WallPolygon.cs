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
                        IfWall.IfDimension.XDim - ((Openings[0].IfLocation.X) + Openings[0].IfDimension.XDim),
                        IfWall.IfDimension.YDim,
                        IfWall.IfDimension.ZDim,
                        (Openings[0].IfLocation.X) + Openings[0].IfDimension.XDim, 0, 0, RegionLocation.Right);

                    Region drl = new Region(
                        Openings[0].IfLocation.X,
                        IfWall.IfDimension.YDim,
                        IfWall.IfDimension.ZDim, 0, 0, 0, RegionLocation.Left);

                    Regions.Add(drr);
                    Regions.Add(drl);

                    switch (Openings[0].OpeningType)
                    {

                        case OpeningType.Door:
                            Region drt = new Region(
                                Openings[0].IfDimension.XDim,
                                IfWall.IfDimension.YDim,
                                IfWall.IfDimension.ZDim - (Openings[0].IfLocation.Z + Openings[0].IfDimension.ZDim),
                                Openings[0].IfLocation.X,
                                0,
                                Openings[0].IfLocation.Z + Openings[0].IfDimension.ZDim, RegionLocation.Top);
                            drt.LocalPlacement = Openings[0].LocalPlacement;
                            Regions.Add(drt);

                            break;
                        case OpeningType.Window:
                            Region wrt = new Region(
                                Openings[0].IfDimension.XDim,
                                IfWall.IfDimension.YDim,
                                IfWall.IfDimension.ZDim - (Openings[0].IfLocation.Z + Openings[0].IfDimension.ZDim),
                                Openings[0].IfLocation.X, 0,
                                Openings[0].IfLocation.Z + Openings[0].IfDimension.ZDim, RegionLocation.Top);
                            wrt.LocalPlacement = Openings[0].LocalPlacement;
                            Region wrb = new Region(
                                Openings[0].IfDimension.XDim,
                                IfWall.IfDimension.YDim,
                                Openings[0].IfLocation.Z,
                                Openings[0].IfLocation.X,
                                0, 0, RegionLocation.Bottom);
                            wrt.LocalPlacement = Openings[0].LocalPlacement;
                            Regions.Add(wrt);
                            Regions.Add(wrb);
                            break;
                    }
                }
                else
                {
                    Region drr = new Region(
                        IfWall.IfDimension.XDim - ((Openings[Openings.Count - 1].IfLocation.X) + Openings[Openings.Count - 1].IfDimension.XDim),
                        IfWall.IfDimension.YDim,
                        IfWall.IfDimension.ZDim,
                        (Openings[Openings.Count - 1].IfLocation.X) + Openings[Openings.Count - 1].IfDimension.XDim,
                        0, 0, RegionLocation.Right);

                    Region drl = new Region(
                        Openings[0].IfLocation.X,
                        IfWall.IfDimension.YDim,
                        IfWall.IfDimension.ZDim,
                        0, 0, 0, RegionLocation.Left);

                    Regions.Add(drr);
                    Regions.Add(drl);

                    for (int i = 0; i < Openings.Count - 1; i++)
                    {
                        Region rbetween = new Region(
                            Openings[i + 1].IfLocation.X - (Openings[i].IfLocation.X + Openings[i].IfDimension.XDim),
                            IfWall.IfDimension.YDim,
                            IfWall.IfDimension.ZDim,
                            Openings[i].IfLocation.X + Openings[i].IfDimension.XDim,
                            0, 0, RegionLocation.Middle);

                        Regions.Add(rbetween);
                    }
                    for (int i = 0; i < Openings.Count; i++)
                    {
                        Region rt = new Region(
                            Openings[i].IfDimension.XDim,
                            IfWall.IfDimension.YDim,
                            IfWall.IfDimension.ZDim - (Openings[i].IfLocation.Z + Openings[i].IfDimension.ZDim),
                            Openings[i].IfLocation.X,
                            0,
                            Openings[i].IfLocation.Z + Openings[i].IfDimension.ZDim, RegionLocation.Top);
                        rt.LocalPlacement = Openings[i].LocalPlacement;
                        Regions.Add(rt);
                        if (Openings[i].OpeningType == OpeningType.Window)
                        {
                            Region rb = new Region(
                                Openings[i].IfDimension.XDim,
                                IfWall.IfDimension.YDim,
                                Openings[i].IfLocation.Z,
                                Openings[i].IfLocation.X,
                                0, 0, RegionLocation.Bottom);
                            rb.LocalPlacement = Openings[i].LocalPlacement;
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
