namespace Bim.Domain
{
    public interface IWindow:IElement
    {
        IDimension Dimensions { get; set; }
        ILocation Location { get; set; }
    }
}