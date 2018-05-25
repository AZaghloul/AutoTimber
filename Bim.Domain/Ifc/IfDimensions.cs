using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.Ifc
{
    public class IfDimension : IDimension
    {
        public double XDim { get; set; }
        public double YDim { get; set; }
        public double ZDim { get; set; }
        public IfDimension()
        {


        }
        public IfDimension(double xDim, double yDim, double zDim)
        {
            XDim = xDim;
            YDim = yDim;
            ZDim = zDim;
        }
    }
}
