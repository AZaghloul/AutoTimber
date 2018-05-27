using Bim.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain
{
    public interface IStory
    {
        int StoryNo { get; set; }
        IBuilding Building { get; set; }
    }
}
