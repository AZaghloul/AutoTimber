using Bim.Domain.Ifc;
using System.Collections.Generic;

namespace Bim.Domain
{
    public interface IWall
    {
        IDimension Dimension { get; set; }
        ILocation Location { get; set; }
        IfStory Story { get; set; }
        List<IDoor> Doors { get; set; }
        List<IWindow> Windows { get; set; }
        List<IOpening> Openings { get; set; }
        bool IsExternal { get; set; }
        
    }
}