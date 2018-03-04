using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Common
{
   public class Window
    {
        public Location Location { get; private set; }
        public Dimension Dimensions { get; private set; }

        public Window(float xDim, float yDim, float height, float x, float y, float z)
        {
            Location = new Location(x, y, z);
            Dimensions = new Dimension(xDim, yDim, height);
        }

        public Window(Dimension dimensions, Location l) : this(dimensions.XDIM, dimensions.YDIM, dimensions.Height, l.X, l.Y, l.Z)
        {


        }

        public Window(Location location) : this(0, 0, 0, location.X, location.Y, location.Z)
        {

        }
        public Window(Dimension dimensions) : this(dimensions.XDIM, dimensions.YDIM, dimensions.Height, 0, 0, 0)
        {

        }
    }
}
