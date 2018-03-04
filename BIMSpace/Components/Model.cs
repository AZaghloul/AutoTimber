using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc;
using Xbim.Ifc.ViewModels;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;
using Xbim.Common;
using Bim.Interfaces;

namespace Bim.Common
{
    /// <summary>
    /// This class imports, inzialize and Handle  the model 
    /// </summary>
   public class Model
    {
        private IfcStore _ifcModel;
        public IfcStore IfcModel
        {
            get { return _ifcModel; }
            set { _ifcModel = value; }
        }


        public void Initialize(IfcStore IfcModel)
        {
            _ifcModel = IfcModel;
        
        }

    }
   
}
