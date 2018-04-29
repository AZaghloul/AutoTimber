using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc;
using Xbim.IO;
using Bim.IO.General;
using Bim.Common;
namespace Bim.IO
{
    public class IfcHandler : IHandler

    {
        private string _filePath;
        private IfcStore model;

        public IfcStore Model
        {
            get { return model; }
            set { model = value; }
        }

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        public string FileName { get; set; }
        public bool IsVisible { get; set; }
        #region Helper Methods

        #endregion

        #region Methods
        public IfcStore Open(string fileName)
        {
            _filePath = fileName;
            model = IfcStore.Open(_filePath);
            return model;
        }
        public void Export()
        {
            model.SaveAs(Path.ChangeExtension(_filePath, "ifcxml"));
        }
        public void Export(string filePath, SaveAs extension)
        {
            switch (extension)
            {
                case SaveAs.Ifc:
                    model.SaveAs(filePath, IfcStorageType.Ifc);

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

        public void Save()
        {
            throw new NotImplementedException();
        }

       

        public void Delete()
        {
            throw new NotImplementedException();
        }



        public void Close()
        {
            throw new NotImplementedException();
        }

        void IHandler.Open(string fileName)
        {
            throw new NotImplementedException();
        }




        #endregion
    }


}
