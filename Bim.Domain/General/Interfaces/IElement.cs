using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain
{
    public interface IElement
    {
        int Id { get; set; }
        int Label { get; set; }
        string Name { get; set; }
        Guid Guid { get; set; }
        string Description { get; set; }
    }
}
