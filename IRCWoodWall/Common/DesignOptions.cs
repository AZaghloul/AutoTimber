using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCWoodWall.Common
{
    public class DesignOptions
    {

        public Option FrameWalls { get; set; }
        public Option FrameFloors { get; set; }
        public Option FrameRafter { get; set; }
        public Option DetectExternalWalls { get; set; }
        public Option OptimizeWalls { get; set; }
        public Option OptimizeFloors { get; set; }
        public Option OptimizeRafter { get; set; }
        public Option Exclude { get; set; }

    }
}
