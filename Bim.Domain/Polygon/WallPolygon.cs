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

namespace Bim.Domain.Polygon
{
    public enum Direction
    {
        XconstYpos,
        XconstYneg,
        XposYconst,
        XnegYconst,
        XposYpos,
        XposYneg,
        XnegYpos,
        XnegYneg
    };
    public class WallPolygon
    {
        public List<IOpening> Opens { get; set; }
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
        public Direction Direction { get; set; }

        public IWall MyWall { get; set; }

        public WallPolygon(IWall MYWall)
        {
            MyWall = MYWall;
            Opens = new List<IOpening>();
            Regions = new List<Region>();
            foreach (var door in MyWall.Doors)
            {
                IOpening open = new Opening(MyWall, door.Dimensions, door.Location, OpeningType.Door);
                Opens.Add(open);
            }
            foreach (var window in MyWall.Windows)
            {
                IOpening open = new Opening(MyWall, window.Dimensions, window.Location, OpeningType.Window);
                Opens.Add(open);
            }
            GetRegions();
        }

        public void GetRegions()
        {
            if (Opens.Count == 0)
            {
                IsOpen = false;
            }
            else
            {
                IsOpen = true;
                List<IOpening> SortedOpens = Opens.OrderBy(open => open.Location.X).ToList();

                if (Opens.Count == 1)
                {
                    Region drr = new Region(
                        MyWall.Dimensions.XDim - ((Opens[0].Location.X) + Opens[0].Dimensions.XDim),
                        MyWall.Dimensions.YDim,
                        MyWall.Dimensions.ZDim,
                        (Opens[0].Location.X) + Opens[0].Dimensions.XDim, 0, 0, RegionLocation.Right);

                    Region drl = new Region(
                        Opens[0].Location.X,
                        MyWall.Dimensions.YDim,
                        MyWall.Dimensions.ZDim, 0, 0, 0, RegionLocation.Left);

                    Regions.Add(drr);
                    Regions.Add(drl);

                    switch (Opens[0].OpeningType)
                    {

                        case OpeningType.Door:
                            Region drt = new Region(
                                Opens[0].Dimensions.XDim,
                                MyWall.Dimensions.YDim,
                                MyWall.Dimensions.ZDim - (Opens[0].Location.Z + Opens[0].Dimensions.ZDim),
                                Opens[0].Location.X,
                                0,
                                Opens[0].Location.Z + Opens[0].Dimensions.ZDim, RegionLocation.Top);

                            Regions.Add(drt);

                            break;
                        case OpeningType.Window:
                            Region wrt = new Region(
                                Opens[0].Dimensions.XDim,
                                MyWall.Dimensions.YDim,
                                MyWall.Dimensions.ZDim - (Opens[0].Location.Z + Opens[0].Dimensions.ZDim),
                                Opens[0].Location.X, 0,
                                Opens[0].Location.Z + Opens[0].Dimensions.ZDim, RegionLocation.Top);

                            Region wrb = new Region(
                                Opens[0].Dimensions.XDim,
                                MyWall.Dimensions.YDim,
                                Opens[0].Location.Z,
                                Opens[0].Location.X,
                                0, 0, RegionLocation.Bottom);

                            Regions.Add(wrt);
                            Regions.Add(wrb);
                            break;
                    }
                }
                else
                {
                    Region drr = new Region(
                        MyWall.Dimensions.XDim - ((Opens[Opens.Count - 1].Location.X) + Opens[Opens.Count - 1].Dimensions.XDim),
                        MyWall.Dimensions.YDim,
                        MyWall.Dimensions.ZDim,
                        (Opens[Opens.Count - 1].Location.X) + Opens[Opens.Count - 1].Dimensions.XDim,
                        0, 0, RegionLocation.Right);

                    Region drl = new Region(
                        Opens[0].Location.X,
                        MyWall.Dimensions.YDim,
                        MyWall.Dimensions.ZDim,
                        0, 0, 0, RegionLocation.Left);

                    Regions.Add(drr);
                    Regions.Add(drl);

                    for (int i = 0; i < Opens.Count - 1; i++)
                    {
                        Region rbetween = new Region(
                            Opens[i + 1].Location.X - (Opens[i].Location.X + Opens[i].Dimensions.XDim),
                            MyWall.Dimensions.YDim,
                            MyWall.Dimensions.ZDim,
                            Opens[i].Location.X + Opens[i].Dimensions.XDim,
                            0, 0, RegionLocation.Middle);

                        Regions.Add(rbetween);
                    }
                    for (int i = 0; i < Opens.Count; i++)
                    {
                        Region rt = new Region(
                            Opens[i].Dimensions.XDim,
                            MyWall.Dimensions.YDim,
                            MyWall.Dimensions.ZDim - (Opens[i].Location.Z + Opens[i].Dimensions.ZDim),
                            Opens[i].Location.X,
                            0,
                            Opens[i].Location.Z + Opens[i].Dimensions.ZDim, RegionLocation.Top);

                        Regions.Add(rt);
                        if (Opens[i].OpeningType == OpeningType.Window)
                        {
                            Region rb = new Region(
                                Opens[i].Dimensions.XDim,
                                MyWall.Dimensions.YDim,
                                Opens[i].Location.Z,
                                Opens[i].Location.X,
                                0, 0, RegionLocation.Bottom);

                            Regions.Add(rb);
                        }
                    }
                }
            }
        }
    }
}


//if (MyWall.Doors != null)
//{
//    if (MyWall.Location.X == MyWall.Doors[0].Location.X & MyWall.Location.Y < MyWall.Doors[0].Location.Y)
//    {
//        Direction = Direction.XconstYpos;
//    }
//    else if (MyWall.Location.X == MyWall.Doors[0].Location.X & MyWall.Location.Y > MyWall.Doors[0].Location.Y)
//    {
//        Direction = Direction.XconstYneg;
//    }
//    else if (MyWall.Location.Y == MyWall.Doors[0].Location.Y & MyWall.Location.X < MyWall.Doors[0].Location.X)
//    {
//        Direction = Direction.XposYconst;
//    }
//    else if (MyWall.Location.Y == MyWall.Doors[0].Location.Y & MyWall.Location.X > MyWall.Doors[0].Location.X)
//    {
//        Direction = Direction.XnegYconst;
//    }
//    else if (MyWall.Location.Y < MyWall.Doors[0].Location.Y & MyWall.Location.X < MyWall.Doors[0].Location.X)
//    {
//        Direction = Direction.XposYpos;
//    }
//    else if (MyWall.Location.Y > MyWall.Doors[0].Location.Y & MyWall.Location.X > MyWall.Doors[0].Location.X)
//    {
//        Direction = Direction.XnegYneg;
//    }
//    else if (MyWall.Location.Y > MyWall.Doors[0].Location.Y & MyWall.Location.X < MyWall.Doors[0].Location.X)
//    {
//        Direction = Direction.XposYneg;
//    }
//    else if (MyWall.Location.Y < MyWall.Doors[0].Location.Y & MyWall.Location.X > MyWall.Doors[0].Location.X)
//    {
//        Direction = Direction.XnegYpos;
//    }
//}
//else if (MyWall.windows != null)
//{
//    if (MyWall.Location.X == MyWall.windows[0].Location.X & MyWall.Location.Y < MyWall.windows[0].Location.Y)
//    {
//        Direction = Direction.XconstYpos;
//    }
//    else if (MyWall.Location.X == MyWall.windows[0].Location.X & MyWall.Location.Y > MyWall.windows[0].Location.Y)
//    {
//        Direction = Direction.XconstYneg;
//    }
//    else if (MyWall.Location.Y == MyWall.windows[0].Location.Y & MyWall.Location.X < MyWall.windows[0].Location.X)
//    {
//        Direction = Direction.XposYconst;
//    }
//    else if (MyWall.Location.Y == MyWall.windows[0].Location.Y & MyWall.Location.X > MyWall.windows[0].Location.X)
//    {
//        Direction = Direction.XnegYconst;
//    }
//    else if (MyWall.Location.Y < MyWall.windows[0].Location.Y & MyWall.Location.X < MyWall.windows[0].Location.X)
//    {
//        Direction = Direction.XposYpos;
//    }
//    else if (MyWall.Location.Y > MyWall.windows[0].Location.Y & MyWall.Location.X > MyWall.windows[0].Location.X)
//    {
//        Direction = Direction.XnegYneg;
//    }
//    else if (MyWall.Location.Y > MyWall.windows[0].Location.Y & MyWall.Location.X < MyWall.windows[0].Location.X)
//    {
//        Direction = Direction.XposYneg;
//    }
//    else if (MyWall.Location.Y < MyWall.windows[0].Location.Y & MyWall.Location.X > MyWall.windows[0].Location.X)
//    {
//        Direction = Direction.XnegYpos;
//    }
//}
