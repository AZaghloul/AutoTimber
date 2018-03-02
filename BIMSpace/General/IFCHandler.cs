using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc;

namespace BIMSpace.General
{
    enum IfcVersion
    {
        Ifc4,
        Ifc3x2,
    }
    class IfcHandler
    {
        private string filePath;
        private IfcStore model;

        public IfcStore Model
        {
            get { return model; }
            set { model = value; }
        }

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

      public  IfcStore open(string filePath)
        {
            this.filePath = filePath;
           model = IfcStore.Open(this.filePath);
            return model;
        }
      public  void Export()
        {
            model.SaveAs(filePath + ".ifcxml");
        }
        public void Export(string filePath)
        {
            model.SaveAs(filePath + ".ifcxml");
        }
    }
}
