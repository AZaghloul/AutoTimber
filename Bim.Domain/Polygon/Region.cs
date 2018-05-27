
using Bim.Domain.Ifc;
using UnitsNet;
using UnitsNet.Units;

    namespace Bim.Domain.Polygon
{
    public enum RegionLocation
    {
        Right,
        Left,
        Middle,
        Top,
        Bottom
    };
    public class Region
    {
        public IfLocation Location { get; set; }
        public IfDimension Dimension { get; set; }
        public RegionLocation RegionLocation { get; set; }

        public Region(double xDim, double yDim, double height, double x, double y, double z, RegionLocation regionLocation )
        {
            Location = new IfLocation(x, y, z);
            Dimension = new IfDimension(xDim, yDim, height);
            RegionLocation = regionLocation;
            
        }

        public Region(double xDim, double yDim, double height, double x, double y, double z)
        {
            Location = new IfLocation(x, y, z);
            Dimension = new IfDimension(xDim, yDim, height);
        }
        public Region(IDimension dimensions, ILocation l) : this(dimensions.XDim, dimensions.YDim, dimensions.ZDim, l.X, l.Y, l.Z)
        {

        }
        public Region(ILocation location) : this(0, 0, 0, location.X, location.Y, location.Z)
        {

        }
        public Region(IDimension dimensions) : this(dimensions.XDim, dimensions.YDim, dimensions.ZDim, 0, 0, 0)
        {

        }
    }
}
