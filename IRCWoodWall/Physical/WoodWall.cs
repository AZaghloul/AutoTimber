using BIM.Common;
using BIM.Components;
using BIMSpace.Polygon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCWoodWall.Physical
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
            Double RegionLength = LR.Dimensions.XDIM;
            Location L = LR.Location;
            Stud firstStud = new Stud
            {
                Location = L,
                Length = LR.Dimensions.Height
            };
            Studs.Add(firstStud);
        }
    }
}
