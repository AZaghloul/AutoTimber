using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMSpace.General
{
   public class Location
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public Location(float x,float y,float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Location()
        {

        }

    }
}
