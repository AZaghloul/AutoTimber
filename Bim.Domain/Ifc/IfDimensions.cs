using Bim.Common.Measures;
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

        public IfDimension ToMeters()
        {
            var d = new IfDimension();
            d.XDim = Length.FromInches(XDim).Meter;
            d.YDim = Length.FromInches(YDim).Meter;
            d.ZDim = Length.FromInches(ZDim).Meter;
            return d;
        }
        public IfDimension ToMilliMeters()
        {
            var d = new IfDimension();
            d.XDim = Length.FromInches(XDim).MilliMeter;
            d.YDim = Length.FromInches(YDim).MilliMeter;
            d.ZDim = Length.FromInches(ZDim).MilliMeter;

            return d;
        }

        public IfDimension ToFeet()
        {
            var d = new IfDimension();
            d.XDim = Length.FromInches(XDim).Feet;
            d.YDim = Length.FromInches(YDim).Feet;
            d.ZDim = Length.FromInches(ZDim).Feet;

            return d;
        }
    }
}
