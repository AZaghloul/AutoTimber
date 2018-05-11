using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bim.Domain;
namespace Bim.Domain.Ifc
{
  abstract public class IfOpening : IOpening
    {
        public ILocation Location { get; set; }
        public IDimension Dimensions { get; set; }
        public OpeningType OpeningType { get; set; }
        public int Id { get ; set ; }
        public string Label { get; set; }
        public IElement WallOrSlap { get ; set; }

        protected IfOpening(IfLocation location, IfDimension dimensions, OpeningType type)
        {
            Location = location;
            Dimensions = dimensions;
            OpeningType = type;
        }
    }
}
