
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
