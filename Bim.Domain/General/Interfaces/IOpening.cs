using Bim.Domain.Ifc;

namespace Bim.Domain
{
    public enum OpeningType
    {
        Door,Window
    }
    public interface IOpening
    {
        IElement HostElement { get; set; }
        IDimension Dimensions { get; set; }
        ILocation Location { get; set; }
        OpeningType OpeningType { get; set; }
    }
    public class Opening : IOpening
    {
        public IElement HostElement { get; set; }
        public IDimension Dimensions { get; set; }
        public ILocation Location { get; set; }
        public OpeningType OpeningType { get; set; }
       
        public Opening()
        {

        }

        public Opening(IWall myWall, IDimension dimensions, ILocation location, OpeningType openingType)
        {
           // WallOrSlap = myWall;
            Dimensions = dimensions;
            Location = location;
            OpeningType = openingType;
        }
    }
}