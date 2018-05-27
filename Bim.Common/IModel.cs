using System.Collections.Generic;

namespace Bim.Common
{
    public interface IModel
    {
       
        void Save(string filePath );
         IVersion Version { get; set; }
         List<IObject> Instances { get; set; }
        public List<IBuilding> Buildings { get; set; }
    }
}