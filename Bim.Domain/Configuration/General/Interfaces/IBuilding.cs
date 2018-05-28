using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain
{
    public interface IBuilding
    {
         List<IStory> Stories { get; set; }
        IModel Model { get; set; }
    }
}
