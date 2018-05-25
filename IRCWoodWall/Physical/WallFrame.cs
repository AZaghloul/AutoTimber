using Bim.Application.IRCWood.IRC;
using Bim.Common.Geometery;
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
        public Dictionary<string, IfDimension> Dimensions { get; set; }
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
            var height = WallPolygon.IfWall.IfDimension.ZDim * 3.28084;
            var s = StudTable.GetSpace(storyNo + 1, height, Dimensions["Stud"]).LastOrDefault();
            var maxdistance = s.Spacing;
            var dim = Dimensions["Stud"];
            foreach (var region in WallPolygon.RLeft)
            {

                var spaces = Split.Distance(region.Dimension.XDim, Convert.ToDouble(maxdistance) * 0.0254); //to inch

                spaces = spaces.Where(e => e > .2).ToList();
                spaces.Insert(0, 0);
                for (int i = 0; i < spaces.Count; i++)
                {
                    if (i == 0) { spaces[i] += 0.05 / 2; };
                    var ifStud = new IfStud(WallPolygon.IfWall)
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

                        IfMaterial = IfMaterial.Defaults["Stud"]

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
            var height = WallPolygon.IfWall.IfDimension.ZDim * 3.28084;
            var s = StudTable.GetSpace(storyNo + 1, height, Dimensions["Stud"]).LastOrDefault();
            var maxdistance = s.Spacing;
            var dim = Dimensions["Stud"];
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
            var height = WallPolygon.IfWall.IfDimension.ZDim * 3.28084;
            var s = StudTable.GetSpace(storyNo + 1, height, Dimensions["Stud"]).LastOrDefault();
            var maxdistance = s.Spacing;
            var dim = Dimensions["Stud"];
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
            var dim = Dimensions["TopPlate"];
            var location = new IfLocation(wd.XDim / 2, 0, wd.ZDim);
            var plate = new IfSill(WallPolygon.IfWall)
            {
                IfLocation = location,
                IfDimension = new IfDimension(wd.XDim, dim.YDim, dim.ZDim),
                IfMaterial = IfMaterial.Defaults["TopPlate"]
                
            };
            // IfMaterial.AttatchTo(plate);
            plate.New();
            plate.IfMaterial.AttatchTo(plate);
        }

        private void SetBottomPlate()
        {
            var wl = WallPolygon.IfWall.IfLocation;
            var wd = WallPolygon.IfWall.IfDimension;
            var dim = Dimensions["BottomPlate"];
            var location = new IfLocation(wd.XDim / 2, 0, -dim.ZDim);
            var plate = new IfSill(WallPolygon.IfWall)
            {
                IfLocation = location,
                IfDimension = new IfDimension(wd.XDim, dim.YDim, dim.ZDim),
                IfMaterial = IfMaterial.Defaults["BottomPlate"]
            };

            //  IfMaterial.AttatchTo(plate);
            plate.New();
            plate.IfMaterial.AttatchTo(plate);
        }
        private void SetTopRegion()
        {
            var wl = WallPolygon.IfWall.IfLocation;
            var wd = WallPolygon.IfWall.IfDimension;
            var dim = Dimensions["Header"];
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
                header.IfMaterial = IfMaterial.Defaults["Header"];
                header.IfMaterial.AttatchTo(header);

            }


        }
        public void New()
        {

            SetTopPlate();
            SetBottomPlate();
            SetLeftRegion();
            //SetBetweenRegion();
            //SetRightRegion();
            SetTopRegion();
        }




    }


}

