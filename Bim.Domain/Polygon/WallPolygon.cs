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
        public List<IOpening> Opens { get ; set; }
        public List<Region> RLeft { get ; set ; }
        public List<Region> RRight { get ; set ; }
        public List<Region> RTop { get ; set ; }
        public List<Region> RBottom { get ; set ; }
        public List<Region> RBetween { get ; set ; }
        public bool IsOpen { get ; set ; }
        public Direction Direction { get ; set ; }

        public IWall MyWall { get; set; }

        public WallPolygon(IWall MYWall)
        {
            MyWall = MYWall;
            Opens = new List<IOpening>();
            RLeft = new List<Region>();
            RRight = new List<Region>();
            RTop = new List<Region>();
            RBottom = new List<Region>();
            RBetween = new List<Region>();
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
                        (Opens[0].Location.X) + Opens[0].Dimensions.XDim, 0, 0);

                    Region drl = new Region(
                        Opens[0].Location.X,
                        MyWall.Dimensions.YDim, 
                        MyWall.Dimensions.ZDim, 0, 0, 0);

                    RRight.Add(drr);
                    RLeft.Add(drl);

                    switch (Opens[0].OpeningType)
                    {

                        case OpeningType.Door:
                            Region drt = new Region(
                                Opens[0].Dimensions.XDim, 
                                MyWall.Dimensions.YDim, 
                                MyWall.Dimensions.ZDim - (Opens[0].Location.Z + Opens[0].Dimensions.ZDim), 
                                Opens[0].Location.X, 
                                0, 
                                Opens[0].Location.Z + Opens[0].Dimensions.ZDim);

                            RTop.Add(drt);

                            break;
                        case OpeningType.Window:
                            Region wrt = new Region(
                                Opens[0].Dimensions.XDim, 
                                MyWall.Dimensions.YDim, 
                                MyWall.Dimensions.ZDim - (Opens[0].Location.Z + Opens[0].Dimensions.ZDim), 
                                Opens[0].Location.X, 0, 
                                Opens[0].Location.Z + Opens[0].Dimensions.ZDim);

                            Region wrb = new Region(
                                Opens[0].Dimensions.XDim, 
                                MyWall.Dimensions.YDim, 
                                Opens[0].Location.Z, 
                                Opens[0].Location.X,
                                0, 0);

                            RTop.Add(wrt);
                            RBottom.Add(wrb);
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
                        0, 0);

                    Region drl = new Region(
                        Opens[0].Location.X, 
                        MyWall.Dimensions.YDim, 
                        MyWall.Dimensions.ZDim,
                        0, 0, 0);

                    RRight.Add(drr);
                    RLeft.Add(drl);

                    for (int i = 0; i < Opens.Count - 1; i++)
                    {
                        Region rbetween = new Region(
                            Opens[i + 1].Location.X - (Opens[i].Location.X + Opens[i].Dimensions.XDim),
                            MyWall.Dimensions.YDim,
                            MyWall.Dimensions.ZDim,
                            Opens[i].Location.X + Opens[i].Dimensions.XDim,
                            0, 0);

                        RBetween.Add(rbetween);
                    }
                    for (int i = 0; i < Opens.Count; i++)
                    {
                        Region rt = new Region(
                            Opens[i].Dimensions.XDim,
                            MyWall.Dimensions.YDim,
                            MyWall.Dimensions.ZDim - (Opens[i].Location.Z + Opens[i].Dimensions.ZDim),
                            Opens[i].Location.X,
                            0,
                            Opens[i].Location.Z + Opens[i].Dimensions.ZDim);

                        RTop.Add(rt);
                        if (Opens[i].OpeningType == OpeningType.Window)
                        {
                            Region rb = new Region(
                                Opens[i].Dimensions.XDim,
                                MyWall.Dimensions.YDim,
                                Opens[i].Location.Z,
                                Opens[i].Location.X,
                                0, 0);

                            RBottom.Add(rb);
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
