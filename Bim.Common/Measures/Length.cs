using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
namespace Bim.Common.Measures
{
    public struct Length
    {
        private double _metricValue;
        private double _initialValue;
        public double Meter { get; set; }
        public double MilliMeter { get; set; }
        public double Feet { get; set; }
        public double Inches { get; set; }
        public double Value { get => _initialValue; }
        public static int RoundValue { get; set; } =4;

        public static Length FromMeters(double value)
        {
            return new Length(value , value);

        }
        public static Length FromMilliMeters(double value)
        {
            return new Length(value /1000, value);

        }
        public static Length FromInches(double value)
        {
            return new Length(value * 0.0254, value);

        }
        public static Length FromFeet(double value)
        {
            return new Length(value * 0.3048, value);
        }
        private Length(double metricValue, double ivalue)
        {
            _metricValue = metricValue;
            _initialValue = ivalue;
            Meter = Round(metricValue, RoundValue);
            MilliMeter = Round(_metricValue * 1000, RoundValue);
            Feet = Round(_metricValue * 3.28084, RoundValue);
            Inches = Round(_metricValue * 39.3701, RoundValue);
        }

    }


}
