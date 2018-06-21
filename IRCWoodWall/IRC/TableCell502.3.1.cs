

using Bim.Common.Measures;
using Bim.Domain.Ifc;

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
    public class TableCell502_3_1
    {
        public double Spacing { get; internal set; }
        public RecSection Section { get; set; }
        public Length Span { get; set; }
        public double SpanToInch { get { return Span.Inches; } }
        public WoodType WoodType { get; internal set; }
        public WoodGrade WoodGrade { get; internal set; }
        public double DeadLoadPsF { get; internal set; }
        public TableCell502_3_1()
        {
            Section = new RecSection();
            Span = new Length();
            WoodType = WoodType.Douglas_fir_larch;
            WoodGrade = WoodGrade.SS;
            DeadLoadPsF = 10;
        }

    }
}
