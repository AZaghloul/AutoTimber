using Bim.Common.Measures;
using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bim.Application.IRCWood.IRC
{
    public class StudCell
    {


        public double Spacing { get; set; }
        public double Height { get; set; }
        public int Floor { get; set; }

        public IfDimension Dimension { get; set; }

        public StudCell()
        {

        }
        public StudCell(double spacing, double height, IfDimension dimension)
        {
            Spacing = spacing;
            Height = height;
            Dimension = dimension;
        }

        public StudCell ToMeters()
        {
            var s = new StudCell()
            {
                Spacing = Length.FromInches(Spacing).Meter,
                Height = Length.FromFeet(Height).Meter,
                Dimension = Dimension.ToMeters(),
                Floor = Floor
            };
            return s;
        }
        public StudCell ToMilliMeter()
        {
            var s = new StudCell()
            {
                Spacing = Length.FromInches(Spacing).MilliMeter,
                Height = Length.FromFeet(Height).MilliMeter,
                Dimension = Dimension.ToMilliMeters(),
                Floor = Floor
            };
            return s;
        }
    }


}

