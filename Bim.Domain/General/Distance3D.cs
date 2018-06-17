using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Common.Geometery
{
   public class Distance3D
    {

       public static IfLocation DivideDistance(IfLocation p1, IfDimension p2)
        {
            var x = p2.XDim / 2;
            var y = p2.YDim / 2;

            return new IfLocation(p1.X + x, p2.YDim + y, 0);
        }
    }
}
