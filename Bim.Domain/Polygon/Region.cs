
using Bim.Domain.Ifc;
using UnitsNet;
using UnitsNet.Units;
using Xbim.Ifc4.Interfaces;

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
        public IfLocation IfLocation { get; set; }
        public IfDimension IfDimension { get; set; }
        public IfDirection IfDirection { get; set; }
        public RegionLocation RegionLocation { get; set; }
        public Direction Direction { get; set; }
        public IIfcLocalPlacement LocalPlacement { get; set; }
        public Region(double xDim, double yDim, double height, double x, double y, double z, RegionLocation regionLocation, Direction dir)
        {
            IfLocation = new IfLocation(x, y, z);
            IfDimension = new IfDimension(xDim, yDim, height);
            RegionLocation = regionLocation;
            Direction = dir;
        }

        public Region(double xDim, double yDim, double height, double x, double y, double z)
        {
            IfLocation = new IfLocation(x, y, z);
            IfDimension = new IfDimension(xDim, yDim, height);
            RegionLocation = RegionLocation.Left;
            IfDirection = new IfDirection(1,0,0);
        }
        public Region(IDimension dimensions, ILocation l) : this(dimensions.XDim.Inches, dimensions.YDim.Inches, dimensions.ZDim.Inches, l.X, l.Y, l.Z)
        {

        }
        public Region(ILocation location) : this(0, 0, 0, location.X, location.Y, location.Z)
        {

        }
        public Region(IDimension dimensions) : this(dimensions.XDim.Inches, dimensions.YDim.Inches, dimensions.ZDim.Inches, 0, 0, 0)
        {

        }
        public Region():this(0,0,0,0,0,0)
        {

        }
        public void Flip(Axis axis)
        {
            switch (axis)
            {
                case Axis.xAxis:
                    IfLocation.X = IfLocation.X - IfDimension.XDim;
                    break;
                case Axis.yAxis:
                    IfLocation.Y = IfLocation.Y - IfDimension.YDim;
                    break;
                case Axis.zAxis:
                    IfLocation.Z = IfLocation.Z - IfDimension.ZDim;
                    break;
                default:
                    IfLocation.X = IfLocation.X - IfDimension.XDim;
                    IfLocation.Y = IfLocation.Y - IfDimension.YDim;
                    IfLocation.Z = IfLocation.Z - IfDimension.ZDim;
                    break;
            }
        }
        public override string ToString()
        {
            return $"{IfDimension} in {IfLocation}";
        }
    }
}
