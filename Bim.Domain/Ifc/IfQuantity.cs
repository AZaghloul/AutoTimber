using Bim.Domain.Ifc.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.QuantityResource;

namespace Bim.Domain.Ifc
{
    public class IfQuantity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public IfUnitEnum IfUnitEnum { get; set; }
        public IfcPhysicalSimpleQuantity IfcQuantity { get; set; }
        public IfQuantity(string name, string value, IfUnitEnum unitType)
        {
            IfUnitEnum = unitType;
        }
    }
}
