using BIM.Common;
using IRCWoodWall.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCWoodWall.Physical
{
    internal class Stud : Blank
    {
        public Stud(Location L, Double length)
        {
            Location = L; Length = length;
        }
        public Stud()
        {
            Orientaion = Orientaion.Vertical;
        }
    }
}
