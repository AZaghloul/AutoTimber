using System;

namespace Bim.Domain
{
    public interface IDoor 
    {
        IDimension Dimensions { get; set; }
        ILocation Location { get; set; }
    }

    

}