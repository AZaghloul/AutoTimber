using System;
using System.IO;
using Xbim.Ifc;
using Xbim.IO;
using Bim.IO.General;
using Xbim.ModelGeometry.Scene;
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
        #region Helper Methods

        #endregion

        #region Methods
        public IfcStore Open(string fileName)
        {
            _filePath = fileName;
            model = IfcStore.Open(_filePath);
            return model;
        }

        public void SaveAs(string filePath, Extension extension)
        {
            switch (extension)
            {
                case Extension.Ifc:
                    model.SaveAs(filePath, IfcStorageType.Ifc);

                    break;
                case Extension.IfcXml:
                    model.SaveAs(filePath, IfcStorageType.IfcXml);

                    break;
                case Extension.IfcZip:
                    model.SaveAs(filePath, IfcStorageType.IfcZip);
                    break;
                case Extension.WexBim:
                    // create wexBim file
                    using (var model = IfcStore.Open(filePath))
                    {
                        var context = new Xbim3DModelContext(model);
                        context.CreateContext();

                        var wexBimFilename = Path.ChangeExtension(filePath, "wexBIM");
                        using (var wexBiMfile = File.Create(wexBimFilename))
                        {
                            using (var wexBimBinaryWriter = new BinaryWriter(wexBiMfile))
                            {
                                model.SaveAsWexBim(wexBimBinaryWriter);
                                wexBimBinaryWriter.Close();
                            }
                            wexBiMfile.Close();
                        }
                    }
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

        #endregion

        public static void ToWexBim(string filePath)
        {
            using (var model = IfcStore.Open(filePath))
            {
                var context = new Xbim3DModelContext(model);
                context.CreateContext();

                var wexBimFilename = Path.ChangeExtension(filePath, "wexBIM");
                using (var wexBiMfile = File.Create(wexBimFilename))
                {
                    using (var wexBimBinaryWriter = new BinaryWriter(wexBiMfile))
                    {
                        model.SaveAsWexBim(wexBimBinaryWriter);
                        wexBimBinaryWriter.Close();
                    }
                    wexBiMfile.Close();
                }
            }
        }

        public static bool CheckFileExist(string filePath)
        {
            return File.Exists(filePath);
        }
    }


}
