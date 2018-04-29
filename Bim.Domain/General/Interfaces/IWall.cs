using System.Collections.Generic;

namespace Bim.Domain
{
    public interface IWall:IElement
    {
        IDimension Dimensions { get; set; }
        List<IDoor> Doors { get; set; }
        bool IsExternal { get; set; }
       
        ILocation Location { get; set; }
    }
}