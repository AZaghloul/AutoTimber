using Bim.Common.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Application.IRCWood.IRC
{
    public class TableCell502_5
    {
        public Length Span { get; set; }
        public double SpanToInch { get { return Span.Inches; } }
        public TimperSection Section { get; set; }
        public int NoOfHeaders { get; set; }
        public int NoOfJackStuds { get; set; }
        public Length BuildingWidth { get; set; }
        public double GroundSnowLoad { get; set; }
        public int StoriesAbove { get; set; }
        public bool FloorBearingClearSpan { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="span">Allowable Span</param>
        /// <param name="section">Joist Section</param>
        /// <param name="NuHeaders">number of Headers</param>
        /// <param name="NuJackStuds">JackStuds Required to support Header</param>
        /// <param name="buildingWidth">Width in Feet</param>
        /// <param name="groundSnowLoad">Load in PSF units</param>
        /// <param name="storiesAbove">no of stories above the header</param>
        /// <param name="floorBearing">True for ClearSpan</param>
        public TableCell502_5(Length span, TimperSection section,int NuHeaders, int NuJackStuds, Length buildingWidth, double groundSnowLoad, int storiesAbove, bool floorBearing)
        {
            Span = span;
            Section = section;
            NoOfHeaders = NuHeaders;
            NoOfJackStuds = NuJackStuds;
            BuildingWidth = buildingWidth;
            GroundSnowLoad = groundSnowLoad;
            StoriesAbove = storiesAbove;
            FloorBearingClearSpan = floorBearing;
        }
        public TableCell502_5()
        {
            BuildingWidth = new Length();
            Span = new Length();
            Section = new TimperSection();
        }
    }
}
