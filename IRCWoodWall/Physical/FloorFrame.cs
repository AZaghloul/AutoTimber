using Bim.Application.Ifc;
using Bim.Application.IRCWood.Physical;
using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCWoodWall.Physical
{
    public class FloorFrame
    {

        public FloorPolygon floorPolygon { get; set; }
        public IfStory IfWall { get; set; }
        public List<IfStud> IfStuds { get; set; }
        public List<IfSill> IfSills { get; set; }
        public List<Plate> Plates { get; set; }

    }
}
