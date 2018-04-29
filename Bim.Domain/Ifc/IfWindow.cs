using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bim.Domain;
namespace Bim.Domain.Ifc
{
   public class IfWindow : IWindow
    {
        public ILocation Location { get;  set; }
        
        public IDimension Dimensions { get; set; }
        public int Id { get ; set ; }
        public string Label { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IfWindow(float xDim, float yDim, float ZDim, float x, float y, float z)
        {
            Location = new IfLocation(x, y, z);
            Dimensions = new IfDimension(xDim, yDim, ZDim);
        }

        public IfWindow(IfDimension dimensions, IfLocation l) : this(dimensions.XDim, dimensions.YDim, dimensions.ZDim, l.X, l.Y, l.Z)
        {


        }

        public IfWindow(IfLocation location) : this(0, 0, 0, location.X, location.Y, location.Z)
        {

        }
        public IfWindow(IfDimension dimensions) : this(dimensions.XDim, dimensions.YDim, dimensions.ZDim, 0, 0, 0)
        {

        }
    }
}
