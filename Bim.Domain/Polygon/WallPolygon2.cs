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
    public class WallPolygon2
    {
        #region Properties
        public List<IfOpening2> Openings { get; set; }
        public List<Region> Regions { get; set; }
        public List<IfLocation> DivisionLines { get; set; }
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
        public IfWall2 IfWall { get; set; }

        #endregion

        #region Constructor
        public WallPolygon2(IfWall2 ifWall)
        {
            IfWall = ifWall;
            Openings = new List<IfOpening2>();
            Regions = new List<Region>();
            Openings = IfWall.Openings;
            DivisionLines = new List<IfLocation>();
            DivisionLines.Add(new IfLocation(0, 0, 0));
            DivisionLines.Add(new IfLocation(IfWall.IfDimension.XDim, 0, 0));
            foreach (var open in Openings)
            {
                DivisionLines.Add(open.IfLocation);
                DivisionLines.Add(new IfLocation(open.IfLocation.X + open.IfDimension.XDim * open.IfDirection.X, 0, 0));
            }
            DivisionLines = DivisionLines.OrderBy(line => line.X.Inches).ToList();

            GetRegions();
        }

        #endregion

        #region Methods
        public void GetRegions()
        {
            Length JoistDepth = Length.FromInches(IfJoist.Setup.Get<RecSection>("RecSection").Depth);
            Length RegionHeight = IfWall.IfDimension.ZDim - JoistDepth - Length.FromInches(2 * 2);
            IfDimension WallDimension = new IfDimension(IfWall.IfDimension.XDim, IfWall.IfDimension.YDim, RegionHeight);

            Regions.Add(new Region(DivisionLines[1].X.Inches, 0, WallDimension.ZDim, 0, 0, 0)
            {
                RegionLocation = RegionLocation.Left
            });
            Regions.Add(new Region(DivisionLines.Last().X.Inches - DivisionLines[DivisionLines.Count - 2].X.Inches, 0, WallDimension.ZDim,
                DivisionLines[DivisionLines.Count - 2].X.Inches, 0, 0)
            {
                RegionLocation = RegionLocation.Right
            });

            for (int i = 1; i < DivisionLines.Count - 2; i++)
            {
                if (i % 2 == 0)
                {
                    Regions.Add(new Region()
                    {
                        IfLocation = new IfLocation(DivisionLines[i].X,0,0),
                        IfDimension= new IfDimension(DivisionLines[i+1].X-DivisionLines[i].X,0,WallDimension.ZDim),
                        RegionLocation = RegionLocation.Middle
                    });
                }
                else
                {
                    IfOpening2 Openi = Openings.Where(Open => Open.IfLocation.X.Inches == DivisionLines[i].X.Inches || Open.IfLocation.X.Inches == DivisionLines[i + 1].X.Inches).FirstOrDefault();

                    switch (Openi.OpeningType)
                    {
                        case OpeningType.Door:
                            Regions.Add(new Region()
                            {
                                IfLocation = new IfLocation(DivisionLines[i].X, 0, Openi.IfDimension.ZDim),
                                IfDimension = new IfDimension(DivisionLines[i + 1].X - DivisionLines[i].X, 0, WallDimension.ZDim - Openi.IfDimension.ZDim),
                                RegionLocation = RegionLocation.Top
                            });
                            break;
                        case OpeningType.Window:
                            Regions.Add(new Region()
                            {
                                IfLocation = 
                                    new IfLocation(DivisionLines[i].X, 0, Openi.IfDimension.ZDim + Openi.IfLocation.Z),
                                IfDimension = 
                                    new IfDimension(DivisionLines[i + 1].X - DivisionLines[i].X, 0, WallDimension.ZDim - Openi.IfDimension.ZDim - Openi.IfLocation.Z),
                                RegionLocation = RegionLocation.Top
                            });

                            Regions.Add(new Region()
                            {
                                IfLocation =
                                    new IfLocation(DivisionLines[i].X, 0, 0),
                                IfDimension =
                                    new IfDimension(DivisionLines[i + 1].X - DivisionLines[i].X, 0, Openi.IfLocation.Z),
                                RegionLocation = RegionLocation.Bottom
                            });
                            break;
                    }

                }
            }


        }
        #endregion
        #region Static Methods


        public static List<WallPolygon2> GetPolygons(List<IfWall2> walls)
        {

            List<WallPolygon2> res = new List<WallPolygon2>();
            foreach (var wall in walls)
            {
                res.Add(new WallPolygon2(wall));
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
