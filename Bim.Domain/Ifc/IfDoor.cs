

using Bim.Common.Measures;

namespace Bim.Domain.Ifc
{
    public class IfDoor : IDoor
    {
        public ILocation Location { get; set; }
        public IDimension Dimensions { get; set; }
        public IfDoor(double xDim, double yDim, double ZDim, double x, double y, double z)
        {
            Location = new IfLocation(x, y, z);
            Dimensions = new IfDimension(xDim, yDim, ZDim);
        }

        public IfDoor(Length xDim, Length yDim, Length ZDim, double x, double y, double z)
        {
            Location = new IfLocation(x, y, z);
            Dimensions = new IfDimension(xDim, yDim, ZDim);
        }

        public IfDoor(IfDimension dimensions, IfLocation l) : this(dimensions.XDim, dimensions.YDim, dimensions.ZDim, l.X, l.Y, l.Z)
        {


        }

        public IfDoor(IfLocation location) : this(0, 0, 0, location.X, location.Y, location.Z)
        {

        }
        public IfDoor(IfDimension dimensions) : this(dimensions.XDim, dimensions.YDim, dimensions.ZDim, 0, 0, 0)
        {

        }

    }
}
