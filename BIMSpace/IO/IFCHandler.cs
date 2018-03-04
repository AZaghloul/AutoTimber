using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc;
using Xbim.IO;
using Bim.Interfaces;

namespace Bim.Common
{
   
    public static class IfcHandler
    {
        private static string _filePath;
        private static IfcStore model;

        public static IfcStore Model
        {
            get { return model; }
            set { model = value; }
        }

        public static string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }


        #region Helper Methods
        #endregion

        #region Methods
        public static IfcStore Open(string filePath)
        {
            _filePath = filePath;
            model = IfcStore.Open(_filePath);
            return model;
        }
        public static void Export()
        {
            model.SaveAs(Path.ChangeExtension(_filePath, "ifcxml"));
          }
        public static void Export(string filePath,SaveAs extension)
        {
            switch (extension)
            {
                case SaveAs.Ifc:
                    model.SaveAs(filePath ,IfcStorageType.Ifc);
                    
                    break;
                case SaveAs.IfcXml:
                    model.SaveAs(filePath, IfcStorageType.IfcXml);
                    break;
                case SaveAs.IfcZip:
                    model.SaveAs(filePath, IfcStorageType.IfcZip);
                    break;
                case SaveAs.WexBim:
                   // model.SaveAsWexBim();
                    break;
                default:
                    break;
            }

        }
        #endregion
    }


}
