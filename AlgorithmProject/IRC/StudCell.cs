using Bim.Domain.Ifc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmProject
{
  public  class StudCell
    {
       

        public string Header { get; set; }
        public int Height { get; set; }
        public int Floor { get; set; } 
        public IfDimension Value { get; set; }

        public StudCell(string header, int height, IfDimension value)
        {

            Header = header;
            Height = height;
            Value = value;
        }


    }
}
