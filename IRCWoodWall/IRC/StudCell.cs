using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Application.IRCWood.IRC
{
    public class StudCell
    {


        public double Spacing { get; set; }
        public double Height { get; set; }
        public int Floor { get; set; }
        public IfDimension Value { get; set; }
        public StudCell(double spacing, double height, IfDimension value)
        {
            Spacing = spacing;
            Height = height;
            Value = value;
        }


    }
}
