using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Common
{
    public class Dimension
    {
        public float XDIM { get; set; }
        public float YDIM { get; set; }
        public float Height { get; set; }
        public Dimension()
        {


        }
        public Dimension(float xDim,float yDim,float height)
        {
            XDIM = xDim;
            YDIM = yDim;
            Height = height;
        }
    }
}
