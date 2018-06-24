using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Bim.Common.Measures
{
    public struct FeetAndInches
    {
        public double Feet { get; set; }
        public double Inches { get; set; }
        public FeetAndInches(double feet, double inches)
        {
            Feet = feet;
            Inches = inches;
        }
    }
    public struct Length :IEquatable<Length>
    {
        private double _metricValue;
        private double _initialValue;
        public double Meter { get; set; }
        public double MilliMeter { get; set; }
        public double Feet { get; set; }
        public FeetAndInches FeetAndInches { get; set; }
        public double Inches { get; set; }
        public double Value { get => _initialValue; }
        public static int RoundValue { get; set; } = 4;

        public static Length FromMeters(double value)
        {
            return new Length(value, value);

        }
        public static Length FromMilliMeters(double value)
        {
            return new Length(value / 1000, value);

        }
        public static Length FromInches(double value)
        {
            return new Length(value * 0.0254, value);

        }
        public static Length FromFeet(double value)
        {
            return new Length(value * 0.3048, value);
        }
        public static Length FromFeetAndInches(double Feet, double Inch)
        {
            return new Length(Feet * 0.3048 + Inch * 0.0254, Feet * 12 + Inch);
        }

        public bool Equals(Length other)
        {
            return this.Inches == other.Inches;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private Length(double metricValue, double ivalue)
        {
            _metricValue = metricValue;
            _initialValue = ivalue;
            Meter = Round(metricValue, RoundValue);
            MilliMeter = Round(_metricValue * 1000, RoundValue);
            Feet = Round(_metricValue * 3.28084, RoundValue);
            Inches = Round(_metricValue * 39.3701, RoundValue);
            FeetAndInches = new FeetAndInches((int)(metricValue * 3.28084), Round(((metricValue * 3.28084 - (int)(metricValue * 3.28084)) * 12), RoundValue));
        }

        public static bool operator== (Length L1 , Length L2)
        {
            return Math.Abs(L1.Inches - L2.Inches) < 0.001;
        }

        public static bool operator !=(Length L1, Length L2)
        {
            return !(L1 == L2);
        }

        public static Length operator -(Length L1, Length L2)
        {
            return Length.FromInches(L1.Inches-L2.Inches);
        }
        public static Length operator +(Length L1, Length L2)
        {
            return Length.FromInches(L1.Inches + L2.Inches);
        }

        public static implicit operator double(Length L)
        {
            return L.Inches;
        }

        public override string ToString()
        {
            return $"{Math.Round(Inches,0)}\"";
        }
    }


}
