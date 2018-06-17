
using Bim.Application.IRCWood.Common;
using Bim.Domain;
using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bim.Application.IRCWood.Physical
{
    enum TimperSections
    {
        _2x3,_2x4,_2x6,_2x8,_2x10,_2x12
    }
    public abstract class Blank
    {
        public double Length { get; set; }
        public WoodSecType SecType { get; set; }
        public IfLocation Location { get; set; }
        protected Orientaion Orientaion { get; set; }

        public Blank()
        {
            SecType = WoodSecType._2x4;
            Location = new IfLocation(0, 0, 0);
        }
    }
}
