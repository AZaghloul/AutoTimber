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
        public string Type { get; set; }
        public int Id { get ; set ; }
        public string Label { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected IfOpening(IfLocation location, IfDimension dimensions,string type)
        {
            Location = location;
            Dimensions = dimensions;
            Type = type;
        }
    }
}
