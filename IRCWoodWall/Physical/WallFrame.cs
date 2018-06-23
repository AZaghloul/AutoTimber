using Bim.Application.IRCWood.IRC;
using Bim.Common.Geometery;
using Bim.Common.Measures;
using Bim.Domain.Ifc;
using Bim.Domain.Polygon;

using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.GeometryResource;

namespace Bim.Application.IRCWood.Physical

{
    public class WallFrame
    {
        public WallPolygon WallPolygon { get; set; }
        public WallPolygon2 WallPolygon2 { get; set; }
        public List<IfStud> IfStuds { get; set; }
        public List<IfSill> IfSills { get; set; }
        public List<IfSill> Headers { get; set; }
        public List<Plate> Plates { get; set; }
        public StudTable StudTable { get; set; }
        public Table502_5 HeadersTable { get; set; }
        public WallFrame(WallPolygon wallPolygon)
        {
            WallPolygon = wallPolygon;
            IfStuds = new List<IfStud>();
            Plates = new List<Plate>();
            IfSills = new List<IfSill>();
            Headers = new List<IfSill>();
        }
        public WallFrame(WallPolygon2 wallPolygon)
        {
            WallPolygon2 = wallPolygon;
            IfStuds = new List<IfStud>();
            Plates = new List<Plate>();
            IfSills = new List<IfSill>();
            Headers = new List<IfSill>();
        }

        private void SetStudsRegions()
        {
            var storyNo = WallPolygon.IfWall.Story.StoryNo;
            double height = 0;
            UnitName unit = WallPolygon.IfWall.IfModel.IfUnit.LengthUnit;
            height = WallPolygon.IfWall.IfDimension.ZDim.Feet;

            var dim = IfStud.Setup.Get<IfDimension>("Dimension");

            var maxdistance = StudTable.GetSpace(storyNo + 1, height, dim)
                .LastOrDefault().Spacing;

            //set Left Region
            foreach (var region in WallPolygon.RLeft)
            {
                double distance = 0;

                distance = region.IfDimension.XDim.Inches;

                var spaces = Split.Equal(distance - dim.XDim.Inches, maxdistance);

                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim.Inches / 2,
                                     region.IfLocation.Y,
                                     region.IfLocation.Z),

                        IfDimension = new IfDimension(
                                       dim.XDim,
                                       dim.YDim,
                                       region.IfDimension.ZDim),

                        IfMaterial = IfMaterial.Setup.Get<IfMaterial>("RLeft"),

                    };

                    ifStud.New();
                    ifStud.IfMaterial.AttatchTo(ifStud);
                    //add to studs elments
                    IfStuds.Add(ifStud);

                    //
                }
            }
            //set right Region
            foreach (var region in WallPolygon.RRight)
            {
                double distance = 0;
                //switch (unit)
                //{
                //    case UnitName.MILLIMETRE:
                distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.FOOT:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.METRE:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    default:
                //        break;
                //}

                var spaces = Split.Equal(distance - dim.XDim.Inches, maxdistance);
                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim.Inches / 2,
                                     region.IfLocation.Y,
                                     region.IfLocation.Z),

                        IfDimension = new IfDimension(
                                       dim.XDim,
                                       dim.YDim,
                                       region.IfDimension.ZDim),

                        IfMaterial = IfMaterial.Setup.Get<IfMaterial>("RRight")
                    };

                    ifStud.New();
                    ifStud.IfMaterial.AttatchTo(ifStud);
                    //add to studs elments
                    IfStuds.Add(ifStud);




                    //
                }
            }
            //set between regions
            foreach (var region in WallPolygon.RBetween)
            {
                double distance = 0;
                //switch (unit)
                //{
                //    case UnitName.MILLIMETRE:
                distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.FOOT:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.METRE:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    default:
                //        break;
                //}

                var spaces = Split.Equal(distance - dim.XDim.Inches, maxdistance);

                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim.Inches / 2,
                                     region.IfLocation.Y,
                                     region.IfLocation.Z),

                        IfDimension = new IfDimension(
                                       dim.XDim,
                                        dim.YDim,
                                       region.IfDimension.ZDim),

                        IfMaterial = IfMaterial.Setup.Get<IfMaterial>("RBetween")
                    };

                    ifStud.New();
                    ifStud.IfMaterial.AttatchTo(ifStud);
                    //add to studs elments
                    IfStuds.Add(ifStud);




                    //
                }
            }
            //set Bottom Region
            foreach (var region in WallPolygon.RBottom)
            {
                double distance = 0;
                //switch (unit)
                //{
                //    case UnitName.MILLIMETRE:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.FOOT:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.METRE:
                distance = region.IfDimension.XDim.Inches;
                //        break;
                //    default:
                //        break;
                //}
                var spaces = Split.Equal(distance - dim.XDim.Inches, maxdistance);
                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim.Inches / 2,
                                     region.IfLocation.Y,
                                     region.IfLocation.Z),

                        IfDimension = new IfDimension(
                                       dim.XDim,
                                       dim.YDim,
                                       region.IfDimension.ZDim),

                        IfMaterial = IfMaterial.Setup.Get<IfMaterial>("BottomStud")
                    };

                    ifStud.New();
                    ifStud.IfMaterial.AttatchTo(ifStud);
                    //add to studs elments
                    IfStuds.Add(ifStud);


                    //
                }
            }
            //set top region
            foreach (var region in WallPolygon.RTop)
            {
                double distance = 0;
                //switch (unit)
                //{
                //    case UnitName.MILLIMETRE:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.FOOT:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.METRE:
                distance = region.IfDimension.XDim.Inches;
                //        break;
                //    default:
                //        break;
                //}
                var spaces = Split.Equal(distance - dim.XDim.Inches, maxdistance);
                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim.Inches / 2,
                                     region.IfLocation.Y,
                                     region.IfLocation.Z),

                        IfDimension = new IfDimension(
                                       dim.XDim,
                                       dim.YDim,
                                       region.IfDimension.ZDim),

                        IfMaterial = IfMaterial.Setup.Get<IfMaterial>("TopStud")
                    };

                    ifStud.New();
                    ifStud.IfMaterial.AttatchTo(ifStud);
                    //add to studs elments
                    IfStuds.Add(ifStud);


                    //
                }
            }
        }
        private void SetStudsRegions2()
        {
            var storyNo = WallPolygon2.IfWall.Story.StoryNo;
            double height = 0;
            height = WallPolygon2.IfWall.IfDimension.ZDim.Feet;

            var dim = IfStud.Setup.Get<IfDimension>("Dimension");

            var maxdistance = StudTable.GetSpace(storyNo + 1, height, dim)
                .LastOrDefault().Spacing;

            //set Left Region
            foreach (var region in WallPolygon2.RLeft)
            {
                double distance = 0;

                distance = region.IfDimension.XDim.Inches;

                var spaces = Split.Equal(distance - dim.XDim.Inches, maxdistance);

                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon2.IfWall)
                    {
                        IfModel = WallPolygon2.IfWall.IfModel,
                        IfWall2 = WallPolygon2.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim.Inches / 2,
                                     region.IfLocation.Y,
                                     region.IfLocation.Z),

                        IfDimension = new IfDimension(
                                       dim.XDim,
                                       dim.YDim,
                                       region.IfDimension.ZDim),

                        IfMaterial = IfMaterial.Setup.Get<IfMaterial>("RLeft"),

                    };

                    ifStud.New();
                    ifStud.IfMaterial.AttatchTo(ifStud);
                    //add to studs elments
                    IfStuds.Add(ifStud);

                    //
                }
            }
            //set right Region
            foreach (var region in WallPolygon2.RRight)
            {
                double distance = 0;
                //switch (unit)
                //{
                //    case UnitName.MILLIMETRE:
                distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.FOOT:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.METRE:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    default:
                //        break;
                //}

                var spaces = Split.Equal(distance - dim.XDim.Inches, maxdistance);
                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon2.IfWall)
                    {
                        IfModel = WallPolygon2.IfWall.IfModel,
                        IfWall2 = WallPolygon2.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim.Inches / 2,
                                     region.IfLocation.Y,
                                     region.IfLocation.Z),

                        IfDimension = new IfDimension(
                                       dim.XDim,
                                       dim.YDim,
                                       region.IfDimension.ZDim),

                        IfMaterial = IfMaterial.Setup.Get<IfMaterial>("RRight")
                    };

                    ifStud.New();
                    ifStud.IfMaterial.AttatchTo(ifStud);
                    //add to studs elments
                    IfStuds.Add(ifStud);




                    //
                }
            }
            //set between regions
            foreach (var region in WallPolygon2.RBetween)
            {
                double distance = 0;
                //switch (unit)
                //{
                //    case UnitName.MILLIMETRE:
                distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.FOOT:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.METRE:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    default:
                //        break;
                //}

                var spaces = Split.Equal(distance - dim.XDim.Inches, maxdistance);

                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon2.IfWall)
                    {
                        IfModel = WallPolygon2.IfWall.IfModel,
                        IfWall2 = WallPolygon2.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim.Inches / 2,
                                     region.IfLocation.Y,
                                     region.IfLocation.Z),

                        IfDimension = new IfDimension(
                                       dim.XDim,
                                        dim.YDim,
                                       region.IfDimension.ZDim),

                        IfMaterial = IfMaterial.Setup.Get<IfMaterial>("RBetween")
                    };

                    ifStud.New();
                    ifStud.IfMaterial.AttatchTo(ifStud);
                    //add to studs elments
                    IfStuds.Add(ifStud);




                    //
                }
            }
            //set Bottom Region
            foreach (var region in WallPolygon2.RBottom)
            {
                double distance = 0;
                //switch (unit)
                //{
                //    case UnitName.MILLIMETRE:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.FOOT:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.METRE:
                distance = region.IfDimension.XDim.Inches;
                //        break;
                //    default:
                //        break;
                //}
                var spaces = Split.Equal(distance - dim.XDim.Inches, maxdistance);
                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon2.IfWall)
                    {
                        IfModel = WallPolygon2.IfWall.IfModel,
                        IfWall2 = WallPolygon2.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim.Inches / 2,
                                     region.IfLocation.Y,
                                     region.IfLocation.Z),

                        IfDimension = new IfDimension(
                                       dim.XDim,
                                       dim.YDim,
                                       region.IfDimension.ZDim),

                        IfMaterial = IfMaterial.Setup.Get<IfMaterial>("BottomStud")
                    };

                    ifStud.New();
                    ifStud.IfMaterial.AttatchTo(ifStud);
                    //add to studs elments
                    IfStuds.Add(ifStud);


                    //
                }
            }
            //set top region
            foreach (var region in WallPolygon2.RTop)
            {
                double distance = 0;
                //switch (unit)
                //{
                //    case UnitName.MILLIMETRE:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.FOOT:
                //        distance = region.IfDimension.XDim.Inches;
                //        break;
                //    case UnitName.METRE:
                distance = region.IfDimension.XDim.Inches;
                //        break;
                //    default:
                //        break;
                //}
                var spaces = Split.Equal(distance - dim.XDim.Inches, maxdistance);
                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon2.IfWall)
                    {
                        IfModel = WallPolygon2.IfWall.IfModel,
                        IfWall2 = WallPolygon2.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim.Inches / 2,
                                     region.IfLocation.Y,
                                     region.IfLocation.Z),

                        IfDimension = new IfDimension(
                                       dim.XDim,
                                       dim.YDim,
                                       region.IfDimension.ZDim),

                        IfMaterial = IfMaterial.Setup.Get<IfMaterial>("TopStud")
                    };

                    ifStud.New();
                    ifStud.IfMaterial.AttatchTo(ifStud);
                    //add to studs elments
                    IfStuds.Add(ifStud);


                    //
                }
            }
        }

        private void SetTopPlate()
        {
            var wl = WallPolygon.IfWall.IfLocation;
            var wd = WallPolygon.IfWall.IfDimension;
            var dim = IfSill.Setup.Get<IfDimension>("Dimension");
            UnitName unit = WallPolygon.IfWall.IfModel.IfUnit.LengthUnit;

            var location = new IfLocation(0, 0, 0);
            var plate = new IfSill(WallPolygon.IfWall)
            {
                IfLocation = location,
                IfDimension = new IfDimension(wd.XDim, dim.YDim, dim.ZDim),
                IfMaterial = IfMaterial.Setup.Get<IfMaterial>("TopPlate"),
                IfModel = WallPolygon.IfWall.IfModel,
                LocalPlacement = WallPolygon.IfWall.LocalPlacement
            };

            plate.New();
            plate.IfMaterial.AttatchTo(plate);
        }
        private void SetTopPlate2()
        {
            var wl = WallPolygon2.IfWall.IfLocation;
            var wd = WallPolygon2.IfWall.IfDimension;
            var dim = IfSill.Setup.Get<IfDimension>("Dimension");
            var zLocation = WallPolygon2.Regions[0].IfDimension.ZDim + WallPolygon2.Regions[0].IfLocation.Z + Length.FromInches(1);
            var location = new IfLocation(0, 0, zLocation);
            var plate = new IfSill(WallPolygon2.IfWall)
            {
                IfLocation = location,
                IfDimension = new IfDimension(dim.XDim, dim.YDim, wd.XDim),
                IfMaterial = IfMaterial.Setup.Get<IfMaterial>("TopPlate"),
                IfModel = WallPolygon2.IfWall.IfModel
            };

            plate.New2();
            plate.IfMaterial.AttatchTo(plate);
        }
        private void SetBottomPlate()
        {
            var wl = WallPolygon.IfWall.IfLocation;
            var wd = WallPolygon.IfWall.IfDimension;
            var dim = IfSill.Setup.Get<IfDimension>("Dimension");
            UnitName unit = WallPolygon.IfWall.IfModel.IfUnit.LengthUnit;

            switch (unit)
            {
                case UnitName.MILLIMETRE:
                    //dim = dim.ToMilliMeters();
                    break;

                case UnitName.METRE:

                    //dim = dim.ToMeters();
                    break;

                default:
                    //dim = dim.ToFeet();
                    break;
            }
            var location = new IfLocation(wd.XDim.Inches / 2, 0, -dim.ZDim.Inches);
            var plate = new IfSill(WallPolygon.IfWall)
            {
                IfLocation = location,
                IfDimension = new IfDimension(wd.XDim, dim.YDim, dim.ZDim),
                IfMaterial = IfMaterial.Setup.Get<IfMaterial>("BottomPlate"),
                IfModel = WallPolygon.IfWall.IfModel,
                LocalPlacement = (IfcLocalPlacement)WallPolygon.IfWall.LocalPlacement
            };

            plate.New();
            plate.IfMaterial.AttatchTo(plate);
        }
        private void SetBottomPlate2()
        {
            var wl = WallPolygon2.IfWall.IfLocation;
            var wd = WallPolygon2.IfWall.IfDimension;
            var dim = IfSill.Setup.Get<IfDimension>("Dimension");
            var zLocation = WallPolygon2.Regions[0].IfLocation.Z + Length.FromInches(1);
            var location = new IfLocation(0, 0, zLocation);
            var plate = new IfSill(WallPolygon2.IfWall)
            {
                IfLocation = location,
                IfDimension = new IfDimension(dim.XDim, dim.YDim, wd.XDim),
                IfMaterial = IfMaterial.Setup.Get<IfMaterial>("TopPlate"),
                IfModel = WallPolygon2.IfWall.IfModel
            };

            plate.New2();
            plate.IfMaterial.AttatchTo(plate);
        }
        private void SetHeaders()
        {
            var wl = WallPolygon.IfWall.IfLocation;
            var wd = WallPolygon.IfWall.IfDimension;
            var dim = IfSill.Setup.Get<IfDimension>("Dimension");
            UnitName unit = WallPolygon.IfWall.IfModel.IfUnit.LengthUnit;

            switch (unit)
            {
                case UnitName.MILLIMETRE:
                    //dim = dim.ToMilliMeters();
                    break;

                case UnitName.METRE:

                    //dim = dim.ToMeters();
                    break;

                default:
                    //dim = dim.ToFeet();
                    break;
            }

            var regions = WallPolygon.RTop;
            foreach (var region in regions)
            {
                var header = new IfSill(WallPolygon.IfWall);
                var l = region.IfLocation;
                var d = region.IfDimension;

                if (region.Direction == Direction.Positive)
                {
                    header.IfLocation = new IfLocation(l.X + d.XDim.Inches / 2, l.Y, l.Z);
                }
                else
                {
                    header.IfLocation = new IfLocation(l.X - d.XDim.Inches / 2, l.Y, l.Z);
                }

                header.IfDimension = new IfDimension(d.XDim.Inches, dim.YDim.Inches, d.ZDim.Inches / 4);
                header.IfModel = WallPolygon.IfWall.IfModel;
                header.LocalPlacement = region.LocalPlacement;
                header.New();
                Headers.Add(header);
                header.IfMaterial = IfMaterial.Setup.Get<IfMaterial>("Header");
                header.IfMaterial.AttatchTo(header);
                region.IfDimension.ZDim -= header.IfDimension.ZDim;
                region.IfLocation.Z += header.IfDimension.ZDim;
            }


        }
        private void SetHeaders2()
        {
            var wl = WallPolygon2.IfWall.IfLocation;
            var wd = WallPolygon2.IfWall.IfDimension;
            UnitName unit = WallPolygon2.IfWall.IfModel.IfUnit.LengthUnit;
            var regions = WallPolygon.RTop;
            foreach (var region in regions)
            {
                var Cells = HeadersTable.GetCells(region.IfDimension.XDim, Length.FromFeetAndInches(28, 0), 10, WallPolygon2.IfWall.Story.IfBuilding.IfStories.Count - 1 - WallPolygon2.IfWall.Story.StoryNo, true);
                var header = new IfSill(WallPolygon.IfWall);
                var l = region.IfLocation;
                var d = region.IfDimension;
                //Cells[0].NoOfHeaders;
                //Cells[0].NoOfJackStuds;
                //Cells[0].Section.Width;
                //Cells[0].Section.Depth;
                //header.IfDimension = new IfDimension(d.XDim.Inches, dim.YDim.Inches, d.ZDim.Inches / 4);
                header.IfModel = WallPolygon.IfWall.IfModel;
                header.LocalPlacement = region.LocalPlacement;
                header.New();
                Headers.Add(header);
                header.IfMaterial = IfMaterial.Setup.Get<IfMaterial>("Header");
                header.IfMaterial.AttatchTo(header);
                region.IfDimension.ZDim -= header.IfDimension.ZDim;
                region.IfLocation.Z += header.IfDimension.ZDim;
            }
        }

        private void SetWindowSills()
        {
            var wl = WallPolygon.IfWall.IfLocation;
            var wd = WallPolygon.IfWall.IfDimension;
            var dim = IfSill.Setup.Get<IfDimension>("Dimension");
            UnitName unit = WallPolygon.IfWall.IfModel.IfUnit.LengthUnit;

            switch (unit)
            {
                case UnitName.MILLIMETRE:
                    //dim = dim.ToMilliMeters();
                    break;

                case UnitName.METRE:

                    //dim = dim.ToMeters();
                    break;

                default:
                    //dim = dim.ToFeet();
                    break;
            }

            var regions = WallPolygon.RBottom;
            foreach (var region in regions)
            {
                var header = new IfSill(WallPolygon.IfWall);
                var l = region.IfLocation;
                var d = region.IfDimension;

                if (region.Direction == Direction.Positive)
                {
                    header.IfLocation = new IfLocation(l.X + d.XDim.Inches / 2, l.Y, l.Z);
                }
                else
                {
                    header.IfLocation = new IfLocation(l.X - d.XDim.Inches / 2, l.Y, l.Z);
                }

                header.IfDimension = new IfDimension(d.XDim.Inches, dim.YDim.Inches, d.ZDim.Inches / 4);
                header.IfModel = WallPolygon.IfWall.IfModel;
                header.LocalPlacement = region.LocalPlacement;
                header.New();
                Headers.Add(header);
                header.IfMaterial = IfMaterial.Setup.Get<IfMaterial>("Header");
                header.IfMaterial.AttatchTo(header);
                region.IfDimension.ZDim -= header.IfDimension.ZDim;
                region.IfLocation.Z += header.IfDimension.ZDim;
            }


        }
        private void SetJackStuds() { }
        public void New()
        {

            SetTopPlate();
            SetHeaders();
            SetBottomPlate();
            SetStudsRegions();

        }
        public void New2()
        {

            SetTopPlate2();
            //SetHeaders2();
            SetBottomPlate2();
            SetStudsRegions2();

        }


    }


}

