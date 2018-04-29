namespace Bim.Domain
{
    public interface IOpening:IElement
    {
        IDimension Dimensions { get; set; }
        ILocation Location { get; set; }
        string Type { get; set; }
    }
}