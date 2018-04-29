using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Application
{
    public interface IElement
    {
        int Id { get; set; }
        string Label { get; set; }
    }
}
