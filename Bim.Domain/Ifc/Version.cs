using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Common.Step21;

namespace Bim.Domain.Ifc
{
   public class Version
    {
        public IfModel IfModel { get; set; }
        public string CurrentVersion { get; set; }

        public void Upgrade()
        {

            using (var tx = IfModel.IfcStore.BeginTransaction("upgrading File"))
            {
                var ifcModel = IfModel.IfcStore.Header.FileSchema =
                    new StepFileSchema(IfcSchemaVersion.Ifc4);
                tx.Commit();
            }
            Initialize();
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

        public Version(IfModel ifModel)
        {
            IfModel = ifModel;
            Initialize();
        }

        private void Initialize()
        {
            CurrentVersion= IfModel.IfcStore.Header.FileSchema.
                Schemas.FirstOrDefault();
        }
    }
}
