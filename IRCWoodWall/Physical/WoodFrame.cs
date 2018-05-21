
using Bim.Application.Ifc;
using Bim.Domain.Ifc;
using Bim.Domain.Polygon;
using IRCWoodWall.Physical;
using System.Collections.Generic;

namespace Bim.Application.IRCWood.Physical
{
    public class WoodFrame
    {
        List<WallFrame> WallFrames { get; set; }
        public List<FloorFrame> FloorFrames { get; set; }

        public WoodFrame()
        {
        }

        
    }

    

    
}