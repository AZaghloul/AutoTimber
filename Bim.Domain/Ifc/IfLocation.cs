using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bim.Domain;
namespace Bim.Domain.Ifc
{
   public class IfLocation : ILocation
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public IfLocation(double x,double y,double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public IfLocation()
        {

        }

    }
}
