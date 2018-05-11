using IRCWoodWall.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIM.Common;

namespace IRCWoodWall.Physical
{
    internal abstract class Blank
    {
        internal double Length { get; set; }
        internal WoodSecType SecType { get; set; }
        internal Location Location { get; set; }
        protected Orientaion Orientaion { get; set; }

        public Blank()
        {
            SecType = WoodSecType._2x4;
            Location = new Location(0, 0, 0);
        }
    }
}
