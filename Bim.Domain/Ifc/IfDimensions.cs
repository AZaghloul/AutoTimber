using Bim.Common.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.Ifc
{
    public class IfDimension : IDimension, IEquatable<IfDimension>
    {
        public Length XDim { get; set; }
        public Length YDim { get; set; }
        public Length ZDim { get; set; }
        public IfDimension()
        {


        }
        public IfDimension(IfDimension dimension)
        {
            XDim = dimension.XDim;
            YDim = dimension.YDim;
            ZDim = dimension.ZDim;

        }
        public IfDimension(Length xDim, Length yDim, Length zDim)
        {
            XDim = xDim;
            YDim = yDim;
            ZDim = zDim;
        }
        public IfDimension(double xDim, double yDim, double zDim)
        {
            XDim = Length.FromInches(xDim);
            YDim = Length.FromInches(yDim);
            ZDim = Length.FromInches(zDim);
        }


        /*public IfDimension ToMeters()
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

        public IfDimension ToInch()
        {
            var d = new IfDimension();
            d.XDim = Length.FromFeet(XDim).Inches;
            d.YDim = Length.FromFeet(YDim).Inches;
            d.ZDim = Length.FromFeet(ZDim).Inches;
            return d;
        }*/

        public override bool Equals(object obj)
        {
            return Equals(obj as IfDimension);
        }

        public bool Equals(IfDimension other)
        {
            return other != null &&
                   XDim == other.XDim &&
                   YDim == other.YDim &&
                   ZDim == other.ZDim;
        }

        public override int GetHashCode()
        {
            var hashCode = -69891878;
            hashCode = hashCode * -1521134295 + XDim.GetHashCode();
            hashCode = hashCode * -1521134295 + YDim.GetHashCode();
            hashCode = hashCode * -1521134295 + ZDim.GetHashCode();
            return hashCode;
        }

        public static IfDimension operator -(IfDimension d1, IfDimension d2)
        {
            return new IfDimension(d1.XDim - d2.XDim, d1.YDim - d2.YDim, d1.ZDim - d2.ZDim);

        }

        public static bool operator ==(IfDimension d1, IfDimension d2)
        {
            return Math.Abs(d1.XDim.MilliMeter - d2.XDim.MilliMeter) < 0.001 && Math.Abs(d1.YDim.MilliMeter - d2.YDim.MilliMeter) < 0.001 && Math.Abs(d1.ZDim.MilliMeter - d2.ZDim.MilliMeter) < 0.001;
        }
        public static bool operator !=(IfDimension d1, IfDimension d2)
        {
            return !(d1 == d2);
        }

        public override string ToString()
        {
            return $"{XDim} x {YDim} x {ZDim}";
        }
    }
}
