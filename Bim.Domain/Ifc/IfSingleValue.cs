using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc4.PropertyResource;

namespace Bim.Domain.Ifc
{
    public class IfSingleValue
    {
        public IfSingleValue(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public IfcPropertySingleValue IfcSVProperty { get; set; }
    }
}
