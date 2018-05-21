using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCWoodWall.IRC
{
    class StudTable<THeader>
    {
        public List<THeader> MyProperty { get; set; }

        public StudTable()
        {

        }

        public bool LoadTable(string filePath)
        {


            return true;

        }

    }
}
