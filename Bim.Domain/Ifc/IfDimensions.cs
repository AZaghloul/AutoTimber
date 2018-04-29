using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.Ifc
{
    public class IfDimension : IDimension
    {
        public float XDim { get; set; }
        public float YDim { get; set; }
        public float ZDim { get; set; }
        public IfDimension()
        {


        }
        public IfDimension(float xDim, float yDim, float zDim)
        {
            XDim = xDim;
            YDim = yDim;
            ZDim = zDim;
        }
    }
}
