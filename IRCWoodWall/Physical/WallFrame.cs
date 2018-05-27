using Bim.Application.IRCWood.IRC;
using Bim.Common.Geometery;
using Bim.Common.Measures;
using Bim.Domain.Ifc;
using Bim.Domain.Polygon;

using System;
using System.Collections.Generic;
using System.Linq;

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

        private void SetLeftRegion()
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

            var maxdistance = StudTable.GetSpace(storyNo + 1, height, dim)
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
                        distance = region.Dimension.XDim;
                        break;
                    case UnitName.FOOT:
                        distance = Length.FromInches(region.Dimension.XDim).Feet;
                        break;
                    case UnitName.METRE:
                        distance = region.Dimension.XDim;
                        break;
                    default:
                        break;
                }

                var spaces = Split.Distance(dim.XDim / 2,distance, maxdistance, maxdistance);

                
                

                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.Location.X + spaces[i],
                                     region.Location.Y,
                                     region.Location.Z),

                        IfDimension = new IfDimension(
                                       dim.XDim,
                                        dim.YDim,
                                       region.Dimension.ZDim),

                        IfMaterial = IfMaterial.Setup.Get<IfMaterial>("RLeft")
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
                        distance = region.Dimension.XDim;
                        break;
                    case UnitName.FOOT:
                        distance = Length.FromInches(region.Dimension.XDim).Feet;
                        break;
                    case UnitName.METRE:
                        distance = region.Dimension.XDim;
                        break;
                    default:
                        break;
                }

                var spaces = Split.Distance(dim.XDim / 2,distance, maxdistance, maxdistance);

                
                

                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.Location.X + spaces[i],
                                     region.Location.Y,
                                     region.Location.Z),

                        IfDimension = new IfDimension(
                                       dim.XDim,
                                        dim.YDim,
                                       region.Dimension.ZDim),

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
                        distance = region.Dimension.XDim;
                        break;
                    case UnitName.FOOT:
                        distance = Length.FromInches(region.Dimension.XDim).Feet;
                        break;
                    case UnitName.METRE:
                        distance = region.Dimension.XDim;
                        break;
                    default:
                        break;
                }

                var spaces = Split.Distance(dim.XDim / 2, distance, maxdistance, maxdistance);




                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifStud = new IfStud(WallPolygon.IfWall)
                    {
                        IfModel = WallPolygon.IfWall.IfModel,
                        IfWall = WallPolygon.IfWall,
                        IfLocation =
                                     new IfLocation(region.Location.X + spaces[i],
                                     region.Location.Y,
                                     region.Location.Z),

                        IfDimension = new IfDimension(
                                       dim.XDim,
                                        dim.YDim,
                                       region.Dimension.ZDim),

                        IfMaterial = IfMaterial.Setup.Get<IfMaterial>("RBetween")
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
            var s = StudTable.GetSpace(storyNo + 1, height, dim).LastOrDefault();
            var maxdistance = s.Spacing;

            foreach (var region in WallPolygon.RBetween)
            {

                var spaces = Split.Distance(region.Dimension.XDim, Convert.ToDouble(maxdistance) * 0.0254); //to inch

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
                                     new IfLocation(region.Location.X + spaces[i],
                                     region.Location.Y,
                                     region.Location.Z),

                        IfDimension = new IfDimension(
                                       .05f,
                                        .4f,
                                       region.Dimension.ZDim),



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
            var maxdistance = StudTable.GetSpace(storyNo + 1, height, dim).LastOrDefault().Spacing;


            foreach (var region in WallPolygon.RRight)
            {

                var spaces = Split.Distance(region.Dimension.XDim, Convert.ToDouble(maxdistance) * 0.0254); //to inch

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
                                     new IfLocation(region.Location.X + spaces[i],
                                     region.Location.Y,
                                     region.Location.Z),

                        IfDimension = new IfDimension(
                                       .05f,
                                        .4f,
                                       region.Dimension.ZDim),
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
                IfModel = WallPolygon.IfWall.IfModel
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
                IfModel = WallPolygon.IfWall.IfModel
            };

            plate.New();
            plate.IfMaterial.AttatchTo(plate);
        }
        private void SetTopRegion()
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
                var l = region.Location;
                var d = region.Dimension;
                header.IfLocation = new IfLocation(l.X - d.XDim / 2, l.Y, l.Z);
                header.IfDimension = new IfDimension(d.XDim, dim.YDim, d.ZDim / 2);
                header.New();
                Headers.Add(header);
                header.IfMaterial = IfMaterial.Setup.Get<IfMaterial>("Header");
                header.IfMaterial.AttatchTo(header);
                header.IfModel = WallPolygon.IfWall.IfModel;

            }


        }
        public void New()
        {

            SetTopPlate();
            SetBottomPlate();
            SetLeftRegion();
           
            SetTopRegion();
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

