using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMSpace.General
{
  abstract public class Opening
    {
        public Location Location { get; set; }
        public Dimension Dimensions { get; set; }
        public Opening(Dimension d,Location l)
        {
            
        }

    }
}
