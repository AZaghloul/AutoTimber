using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.Ifc
{
    public class IfColor : IfObject
    {
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }

        public IfColor()
        {

        }
        public IfColor(double r, double g, double b)
        {
            if (r >= 1 || g >= 1 || b >= 1)
            {
                Red = r / 255;
                Green = g / 255;
                Blue = b / 255;
            }
            else
            {
                Red = r;
                Green = g;
                Blue = b;
            }
        }
    }
}
