using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Application.IRCWood.IRC
{
    public enum AreaType
    {
        SleepingArea, LivingArea
    }
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
        public IfDimension Dimension { get; internal set; }
        public WoodType WoodType { get; internal set; }
        public WoodGrade WoodGrade { get; internal set; }
        public AreaType AreaType { get; internal set; }
        public double DeadLoadPsF { get; internal set; }
        public TableCell502_3_1()
        {
            Dimension = new IfDimension(2, 6, 12 * 12 + 6);
            WoodType = WoodType.Douglas_fir_larch;
            WoodGrade = WoodGrade.SS;
            AreaType = AreaType.SleepingArea;
            DeadLoadPsF = 10;
        }


    }
}
