using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.Ifc
{
   public class IfStory : IElement
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public int StoryNo { get; set; }
        List<IfWall> Walls { get; set; }
        public IfModel IfModel { get; set; }
        public IfStory()
        {
            Walls = new List<IfWall>();
           
        }

    }
}
