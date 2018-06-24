using Bim.Application.IRCWood.IRC;
using Bim.Common.Geometery;
using Bim.Common.Measures;
using Bim.Domain.General;
using Bim.Domain.Ifc;
using Bim.Domain.Polygon;
using MathNet.Spatial.Euclidean;
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

            SetJoists();

        }
        public static void FloorSetup()
        {
            IfJoist.Setup.Add("WoodType", WoodType.Douglas_fir_larch);
            IfJoist.Setup.Add("WoodGrade", WoodGrade.SS);
        }
        private void SetJoists()
        {
            RecSection section = IfJoist.Setup.Get<RecSection>("RecSection");
            WoodType WT = IfJoist.Setup.Get<WoodType>("WoodType");
            WoodGrade WG = IfJoist.Setup.Get<WoodGrade>("WoodGrade");
            foreach (var reg in FloorPolygon.Regions)
            {
                double span = reg.IfDimension.XDim.Inches;
                var Cells = JoistTable.Cells.Where(e =>
                    e.WoodGrade == WG &&
                    e.WoodType == WT &&
                    e.SpanToInch >= span &&
                    e.DeadLoadPsF == 10 &&
                    e.Section ==  section)
                    .OrderBy(e =>
                    e.SpanToInch).ToList();
                double S = Cells.FirstOrDefault().Spacing;
                var spaces = Split.Equal(reg.IfDimension.YDim - section.Width, S);

                for (int i = 0; i < spaces.Count; i++)
                {
                    //var spacingVec = new Vector3D(spaces[i], spaces[i], spaces[i]);
                    var DircVec = new IfLocation(
                        FloorPolygon.IfFloor.ShortDirection.Y * spaces[i],
                        FloorPolygon.IfFloor.ShortDirection.X * spaces[i],
                        0 //FloorPolygon.IfFloor.ShortDirection.Z * spaces[i] - Cells[0].Section.Depth.Inches / 2
                        );

                    var ifJoist = new IfJoist(FloorPolygon.IfFloor)
                    {
                        IfModel = FloorPolygon.IfFloor.IfModel,
                        IfFloor = FloorPolygon.IfFloor,
                        IfLocation = DircVec,

                        IfDimension = new IfDimension(
                                       section.Width.Inches,
                                        section.Depth.Inches,
                                       reg.IfDimension.XDim.Inches),

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
