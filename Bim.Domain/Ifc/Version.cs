using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Common.Step21;
using Xbim.Ifc;

namespace Bim.Domain.Ifc
{
   public class IfVersion:IVersion
    {
        public IfModel IfModel { get; set; }
        public string CurrentVersion { get; set; }
        public IModel Model
        {
            get { return IfModel; }
            set { IfModel = (IfModel)Model; }
        }

        public void Upgrade()
        {

            using (var tx = IfModel.IfcStore.BeginTransaction("upgrading File"))
            {
                var ifcModel = IfModel.IfcStore.Header.FileSchema =
                    new StepFileSchema(IfcSchemaVersion.Ifc4);
                tx.Commit();
            }
            CurrentVersion= CurrentVersion = IfModel.IfcStore.Header.FileSchema.
                Schemas.FirstOrDefault(); 
        }
        public void Downgrade()
        {

            using (var tx = IfModel.IfcStore.BeginTransaction("downgrading File"))
            {
                var ifcModel = IfModel.IfcStore.Header.FileSchema =
                    new StepFileSchema(IfcSchemaVersion.Ifc2X3);
                tx.Commit();
            }
            Initialize();
        }

        public IfVersion(IfModel ifModel)
        {
            IfModel = ifModel;
            Initialize();
        }

        private void Initialize()
        {
            CurrentVersion= IfModel.IfcStore.Header.FileSchema.
                Schemas.FirstOrDefault();

            if (CurrentVersion != "IFC4")
            {
                Upgrade();
                var name = IfModel.IfcStore.FileName;
                IfModel.IfcStore.SaveAs(name);
               IfModel.IfcStore = IfcStore.Open(name);
            }
        }
    }
}
