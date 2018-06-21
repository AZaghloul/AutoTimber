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
        public List<IfStud> IfStuds { get; set; }
        public List<IfSill> IfSills { get; set; }
        public List<IfSill> Headers { get; set; }
        public List<Plate> Plates { get; set; }
        public StudTable StudTable { get; set; }
        public WallFrame(WallPolygon wallPolygon)
        {
            WallPolygon = wallPolygon;
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
            switch (unit)
            {
                case UnitName.MILLIMETRE:
                    height = Length.FromMilliMeters(WallPolygon.IfWall.IfDimension.ZDim).Feet;
                    break;

                case UnitName.FOOT:
                    height = WallPolygon.IfWall.IfDimension.ZDim;
                    break;
                case UnitName.METRE:
                    height = Length.FromMeters(WallPolygon.IfWall.IfDimension.ZDim).Feet;
                    break;
                default:
                    break;
            }

            var dim = IfStud.Setup.Get<IfDimension>("Dimension");

            var maxdistance = StudTable.GetSpace(storyNo , height, dim)
                .LastOrDefault().Spacing;

            // set maxdistance unit
            switch (unit)
            {
                case UnitName.MILLIMETRE:
                    maxdistance = Length.FromInches(maxdistance).MilliMeter;
                    dim = dim.ToMilliMeters();
                    break;

                case UnitName.METRE:
                    maxdistance = Length.FromInches(maxdistance).Meter;
                    dim = dim.ToMeters();
                    break;

                default:
                    maxdistance = Length.FromInches(maxdistance).Feet;
                    dim = dim.ToFeet();
                    break;
            }

            //set Left Region
            foreach (var region in WallPolygon.RLeft)
            {
                double distance = 0;
                switch (unit)
                {
                    case UnitName.MILLIMETRE:
                        distance = region.IfDimension.XDim;
                        break;
                    case UnitName.FOOT:
                        distance = region.IfDimension.XDim;
                        break;
                    case UnitName.METRE:
                        distance = region.IfDimension.XDim;
                        break;
                    default:
                        break;
                }

                var spaces = Split.Equal(distance - dim.XDim, maxdistance);
                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim / 2,
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
                switch (unit)
                {
                    case UnitName.MILLIMETRE:
                        distance = region.IfDimension.XDim;
                        break;
                    case UnitName.FOOT:
                        distance = region.IfDimension.XDim;
                        break;
                    case UnitName.METRE:
                        distance = region.IfDimension.XDim;
                        break;
                    default:
                        break;
                }

                var spaces = Split.Equal(distance - dim.XDim, maxdistance);
                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim / 2,
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
                switch (unit)
                {
                    case UnitName.MILLIMETRE:
                        distance = region.IfDimension.XDim;
                        break;
                    case UnitName.FOOT:
                        distance = region.IfDimension.XDim;
                        break;
                    case UnitName.METRE:
                        distance = region.IfDimension.XDim;
                        break;
                    default:
                        break;
                }

                var spaces = Split.Equal(distance - dim.XDim, maxdistance);

                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim / 2,
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
                switch (unit)
                {
                    case UnitName.MILLIMETRE:
                        distance = region.IfDimension.XDim;
                        break;
                    case UnitName.FOOT:
                        distance = region.IfDimension.XDim;
                        break;
                    case UnitName.METRE:
                        distance = region.IfDimension.XDim;
                        break;
                    default:
                        break;
                }
                var spaces = Split.Equal(distance - dim.XDim, maxdistance);
                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim / 2,
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
                switch (unit)
                {
                    case UnitName.MILLIMETRE:
                        distance = region.IfDimension.XDim;
                        break;
                    case UnitName.FOOT:
                        distance = region.IfDimension.XDim;
                        break;
                    case UnitName.METRE:
                        distance = region.IfDimension.XDim;
                        break;
                    default:
                        break;
                }
                var spaces = Split.Equal(distance - dim.XDim, maxdistance);
                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i] + dim.XDim / 2,
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
        private void SetBetweenRegion()
        {

            var storyNo = WallPolygon.IfWall.Story.StoryNo;
            var height = WallPolygon.IfWall.IfDimension.ZDim;
            var dim = IfStud.Setup.Get<IfDimension>("Dimension");
            var s = StudTable.GetSpace(storyNo, height, dim).LastOrDefault();
            var maxdistance = s.Spacing;

            foreach (var region in WallPolygon.RBetween)
            {

                var spaces = Split.Distance(region.IfDimension.XDim, Convert.ToDouble(maxdistance) * 0.0254); //to inch

                spaces = spaces.Where(e => e > .2).ToList();
                spaces.Insert(0, 0);
                for (int i = 0; i < spaces.Count; i++)
                {
                    if (i == 0) { spaces[i] += 0.05 / 2; };
                    IfStud ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i],
                                     region.IfLocation.Y,
                                     region.IfLocation.Z),

                        IfDimension = new IfDimension(
                                       .05f,
                                        .4f,
                                       region.IfDimension.ZDim),



                    };


                    ifStud.New();
                    ifStud.IfMaterial.AttatchTo(ifStud);
                    //add to studs elments
                    IfStuds.Add(ifStud);

                }

            }

        }
        private void SetRightRegion()
        {

            var storyNo = WallPolygon.IfWall.Story.StoryNo;
            var height = WallPolygon.IfWall.IfDimension.ZDim;
            var dim = IfStud.Setup.Get<IfDimension>("Dimension");
            var maxdistance = StudTable.GetSpace(storyNo, height, dim).LastOrDefault().Spacing;


            foreach (var region in WallPolygon.RRight)
            {

                var spaces = Split.Distance(region.IfDimension.XDim, Convert.ToDouble(maxdistance) * 0.0254); //to inch

                spaces = spaces.Where(e => e > .2).ToList();
                spaces.Insert(0, 0);
                for (int i = 0; i < spaces.Count; i++)
                {
                    if (i == 0) { spaces[i] += 0.05 / 2; };
                    IfStud ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.IfLocation.X + spaces[i],
                                     region.IfLocation.Y,
                                     region.IfLocation.Z),

                        IfDimension = new IfDimension(
                                       .05f,
                                        .4f,
                                       region.IfDimension.ZDim),
                    };


                    ifStud.New();
                    ifStud.IfMaterial.AttatchTo(ifStud);
                    //add to studs elments
                    IfStuds.Add(ifStud);
                }
            }
        }
        private void SetTopPlate()
        {
            var wl = WallPolygon.IfWall.IfLocation;
            var wd = WallPolygon.IfWall.IfDimension;
            var dim = IfSill.Setup.Get<IfDimension>("Dimension");
            UnitName unit = WallPolygon.IfWall.IfModel.IfUnit.LengthUnit;

            switch (unit)
            {
                case UnitName.MILLIMETRE:
                    dim = dim.ToMilliMeters();
                    break;
                case UnitName.METRE:
                    dim = dim.ToMeters();
                    break;
                default:
                    dim = dim.ToFeet();
                    break;
            }

            var location = new IfLocation(wd.XDim / 2, 0, wd.ZDim);
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
        private void SetBottomPlate()
        {
            var wl = WallPolygon.IfWall.IfLocation;
            var wd = WallPolygon.IfWall.IfDimension;
            var dim = IfSill.Setup.Get<IfDimension>("Dimension");
            UnitName unit = WallPolygon.IfWall.IfModel.IfUnit.LengthUnit;

            switch (unit)
            {
                case UnitName.MILLIMETRE:
                    dim = dim.ToMilliMeters();
                    break;

                case UnitName.METRE:

                    dim = dim.ToMeters();
                    break;

                default:
                    dim = dim.ToFeet();
                    break;
            }
            var location = new IfLocation(wd.XDim / 2, 0, -dim.ZDim);
            var plate = new IfSill(WallPolygon.IfWall)
            {
                IfLocation = location,
                IfDimension = new IfDimension(wd.XDim, dim.YDim, dim.ZDim),
                IfMaterial = IfMaterial.Setup.Get<IfMaterial>("BottomPlate"),
                IfModel = WallPolygon.IfWall.IfModel,
                LocalPlacement = WallPolygon.IfWall.LocalPlacement
            };

            plate.New();
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
                    dim = dim.ToMilliMeters();
                    break;

                case UnitName.METRE:

                    dim = dim.ToMeters();
                    break;

                default:
                    dim = dim.ToFeet();
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
                    header.IfLocation = new IfLocation(l.X + d.XDim / 2, l.Y, l.Z);
                }
                else
                {
                    header.IfLocation = new IfLocation(l.X - d.XDim / 2, l.Y, l.Z);
                }

                header.IfDimension = new IfDimension(d.XDim, dim.YDim, d.ZDim / 4);
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
                    dim = dim.ToMilliMeters();
                    break;

                case UnitName.METRE:

                    dim = dim.ToMeters();
                    break;

                default:
                    dim = dim.ToFeet();
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
                    header.IfLocation = new IfLocation(l.X + d.XDim / 2, l.Y, l.Z);
                }
                else
                {
                    header.IfLocation = new IfLocation(l.X - d.XDim / 2, l.Y, l.Z);
                }

                header.IfDimension = new IfDimension(d.XDim, dim.YDim, d.ZDim / 4);
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
        private void SetJackStuds(){}
        public void New()
        {

            SetTopPlate();
            SetHeaders();
            SetBottomPlate();
            SetStudsRegions();
            
        }
        private double FromInches(double value)
        {
            double res = 0;
            UnitName unit = WallPolygon.IfWall.IfModel.IfUnit.LengthUnit;

            switch (unit)
            {
                case UnitName.MILLIMETRE:
                    res = Length.FromInches(res).MilliMeter;
                    break;

                case UnitName.METRE:
                    res = Length.FromInches(res).Meter;
                    break;

                default:
                    break;
            }
            return res;

        }

    }


}

