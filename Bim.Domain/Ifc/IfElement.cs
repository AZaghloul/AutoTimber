using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.Ifc
{
   public abstract class IfElement : IElement
    {
        public int Id { get ; set ; }
        public int Label { get ; set ; }
        public string  Name { get; set; }
        public Guid Guid { get; set; }
        public string Description { get; set ; }
        public IfModel IfModel { get; set; }
        public IfDimension IfDimension { get; set; }
        public IfLocation IfLocation { get; set; }
    }
}
