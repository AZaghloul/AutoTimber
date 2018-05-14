using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc;
using Xbim.Ifc.ViewModels;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;
using Xbim.Common;

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
        public ILocation Location { get; set; }
        public IDimension Dimensions { get; set; }
        public RegionLocation RegionLocation { get; set; }

        public Region(float xDim, float yDim, float height, float x, float y, float z, RegionLocation regionLocation )
        {
            Location = new Location(x, y, z);
            Dimensions = new Dimension(xDim, yDim, height);
            RegionLocation = regionLocation;
        }

        public Region(float xDim, float yDim, float height, float x, float y, float z)
        {
            Location = new Location(x, y, z);
            Dimensions = new Dimension(xDim, yDim, height);
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
