
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bim.Domain.Polygon;
using Bim.Domain;

namespace Bim.Application.IRCWood.Physical
{
    class WoodWall
    {
        public List<Stud> Studs { get; set; }
        public List<Plate> Plates { get; set; }
        public WallPolygon WallPolygon { get; set; }
        public WoodWall(WallPolygon wallPolygon)
        {
            WallPolygon = wallPolygon;
        }
        void SetStudsInLRegion()
        {
            Region LR = WallPolygon.RLeft[0];
            Double RegionLength = LR.IfDimension.XDim;
            // Location L = LR.IfLocation;
            Stud firstStud = new Stud
            {
                Length = LR.IfDimension.ZDim
            };
            Studs.Add(firstStud);

        }
    }
}
