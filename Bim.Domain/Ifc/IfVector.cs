using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.Ifc
{
   public class IfVector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public IfVector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public IfVector()
        {

        }
        
    }
}
