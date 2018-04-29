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
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public IfLocation(float x,float y,float z)
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
