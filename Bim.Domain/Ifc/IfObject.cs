using Bim.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.Ifc
{
  public abstract class IfObject:IObject
    {
        public int Id { get; set; }
        public int Label { get; set; }
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public string Description { get; set; }
    }
}
