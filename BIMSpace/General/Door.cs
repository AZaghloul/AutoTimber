using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMSpace.General
{
   public class Door
    {
        public Location Location { get; set; }
        public Dimension Dimensions { get; set; }
        public Door(float xDim, float yDim, float height, float x, float y, float z)
        {
            Location = new Location(x, y, z);
            Dimensions = new Dimension(xDim, yDim, height);
        }

        public Door(Dimension dimensions, Location l) : this(dimensions.XDIM, dimensions.YDIM, dimensions.Height, l.X, l.Y, l.Z)
        {


        }

        public Door(Location location) : this(0, 0, 0, location.X, location.Y, location.Z)
        {

        }
        public Door(Dimension dimensions) : this(dimensions.XDIM, dimensions.YDIM, dimensions.Height, 0, 0, 0)
        {

        }

    }
}
