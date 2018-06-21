using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bim.Common.Measures;
using Bim.Domain;
namespace Bim.Domain.Ifc
{
   public class IfLocation : ILocation
    {
        public Length X { get; set; }
        public Length Y { get; set; }
        public Length Z { get; set; }
        public IfLocation(double x,double y,double z)
        {
            X = Length.FromInches(x);
            Y = Length.FromInches(y);
            Z = Length.FromInches(z);
        }
        public IfLocation(Length x, Length y, Length z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public IfLocation():this(0,0,0)
        {

        }
        public IfLocation(IfLocation location)
        {
            X = location.X;
            Y = location.Y;
            Z = location.Z;
        }

        public override string ToString()
        {
            return $"{X.Inches} x {Y.Inches} x {Z.Inches}";
        }

    }
}
