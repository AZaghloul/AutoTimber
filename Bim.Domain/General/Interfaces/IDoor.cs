using System;

namespace Bim.Domain
{
    public interface IDoor : IElement
    {
        IDimension Dimensions { get; set; }
        ILocation Location { get; set; }
    }

    

}