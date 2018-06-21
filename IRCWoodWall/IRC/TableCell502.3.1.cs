using Bim.Common.Measures;
using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Application.IRCWood.IRC
{
    public enum WoodType
    {
        Douglas_fir_larch, Hem_fir, Southern_pine, Spruce_pine_fir
    }
    public enum WoodGrade
    {
        SS,
        _1,
        _2,
        _3,
        _4
    }
    public class TimperSection
    {
        public double Width { get; set; }
        public double Depth { get; set; }
        public TimperSection(double Width, double Depth)
        {
            this.Width = Width;
            this.Depth = Depth;
        }
        public TimperSection() : this(0, 0) { }
    }
    public class TableCell502_3_1
    {
        public double Spacing { get; internal set; }
        public TimperSection Section { get; set; }
        public Length Span { get; set; }
        public double SpanToInch { get { return Span.Inches; } }
        public WoodType WoodType { get; internal set; }
        public WoodGrade WoodGrade { get; internal set; }
        public double DeadLoadPsF { get; internal set; }
        public TableCell502_3_1()
        {
            Section = new TimperSection();
            Span = new Length();
            WoodType = WoodType.Douglas_fir_larch;
            WoodGrade = WoodGrade.SS;
            DeadLoadPsF = 10;
        }

    }
}
