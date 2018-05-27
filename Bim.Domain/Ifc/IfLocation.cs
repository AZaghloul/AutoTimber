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
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public IfLocation(double x,double y,double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public IfLocation()
        {

        }

        public IfLocation ToMeters()
        {
            var d = new IfLocation();
            d.X = Length.FromInches(X).Meter;
            d.Y = Length.FromInches(Y).Meter;
            d.Z = Length.FromInches(Z).Meter;
            return d;
        }
        public IfLocation ToMilliMeters()
        {
            var d = new IfLocation();
            d.X = Length.FromInches(X).MilliMeter;
            d.Y = Length.FromInches(Y).MilliMeter;
            d.Z = Length.FromInches(Z).MilliMeter;

            return d;
        }

    }
}
