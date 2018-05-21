using Bim.Application.Ifc;
using Bim.Application.IRCWood.Physical;
using Bim.Domain.Ifc;
using Bim.Domain.Polygon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCWoodWall.Physical
{
    public class WallFrame
    {
        public WallPolygon WallPolygon { get; set; }
        public IfWall IfWall { get; set; }
        public List<IfStud> IfStuds { get; set; }
        public List<IfSill> IfSills { get; set; }
        public List<Plate> Plates { get; set; }

    }
}
