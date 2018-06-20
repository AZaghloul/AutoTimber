using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bim.Common;
using Xbim.Ifc;

namespace Bim.IO.General
{
    public interface IHandler
    {
        string FileName { get; set; }
        
        void Save();
        IfcStore Open(string fileName);
        void Delete();
       
        void Close();
    }
}
