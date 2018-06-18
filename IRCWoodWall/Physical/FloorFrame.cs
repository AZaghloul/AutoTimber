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
    public class FloorFrame
    {
        public FloorPolygon FloorPolygon { get; set; }
        public IfStory IfStory { get; set; }
        public List<IfJoist> Joists { get; set; }
        public List<IfJoist> Girders { get; set; }
        public Table502_3_1 JoistTable { get; set; }
        public FloorFrame(FloorPolygon floorPolygon)
        {
            FloorPolygon = floorPolygon;
            Joists = new List<IfJoist>();
            Girders = new List<IfJoist>();
        }

        public void New()
        {

            //SetHeaders();
            IfJoist.Setup = new Domain.Configuration.Setup
            {
                {"RecSection",new RecSection(2,10) },
                {"WoodType", WoodType.Douglas_fir_larch },
                {"WoodGrade", WoodGrade.SS }
            };

            SetJoists();

        }

        private void SetJoists()
        {
            RecSection section = IfJoist.Setup.Get<RecSection>("RecSection");
            WoodType WT = IfJoist.Setup.Get<WoodType>("WoodType");
            WoodGrade WG = IfJoist.Setup.Get<WoodGrade>("WoodGrade");
            foreach (var reg in FloorPolygon.Regions)
            {
                double span = reg.IfDimension.XDim;
                var Cells = JoistTable.Cells.Where(e =>
                    e.WoodGrade == WG &&
                    e.WoodType == WT &&
                    e.SpanToInch >= span*12 &&
                    e.DeadLoadPsF == 10 &&
                    e.Section == section)
                    .OrderBy(e =>
                    e.SpanToInch).ToList();
                double S = Cells[0].Spacing/12;
                var spaces = Split.Equal(reg.IfDimension.YDim, S);

                for (int i = 0; i < spaces.Count; i++)
                {
                    var ifJoist = new IfJoist(FloorPolygon.IfFloor)
                    {
                        IfModel = FloorPolygon.IfFloor.IfModel,
                        IfFloor = FloorPolygon.IfFloor,
                        IfLocation =
                                     new IfLocation(reg.IfLocation.X + spaces[i] + section.Width.Inches/2,
                                     reg.IfLocation.Y,
                                     reg.IfLocation.Z),

                        IfDimension = new IfDimension(
                                       section.Width.Inches,
                                        section.Depth.Inches,
                                       reg.IfDimension.XDim),

                        IfMaterial = IfMaterial.Setup.Get<IfMaterial>("Joist")
                    };

                    ifJoist.New();
                    ifJoist.IfMaterial.AttatchTo(ifJoist);
                    //add to studs elments
                    Joists.Add(ifJoist);




                    //
                }

            }
        }

        private void SetHeaders()
        {
        }
    }
}
