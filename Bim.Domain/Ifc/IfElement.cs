using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc4.ProductExtension;

namespace Bim.Domain.Ifc
{
   public abstract class IfElement :IfObject, IElement
    {
        
        public IfModel IfModel { get; set; }
        public IfcBuildingElement IfcElement { get; set; }
        public IfDimension IfDimension { get; set; }
        public IfLocation IfLocation { get; set; }
        public IfMaterial IfMaterial { get; set; }

        //public override string ToString()
        //{
        //    return $"GeneralElement {IfDimension.XDim} × {IfDimension.YDim} × {IfDimension.ZDim}";
        //}
    }
}
