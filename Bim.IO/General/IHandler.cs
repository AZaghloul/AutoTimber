using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bim.Common;
namespace Bim.IO.General
{
    public interface IHandler
    {
        string FileName { get; set; }
        
        void Save();
        void Open(string fileName);
        void Delete();
       
        void Close();
    }
}
