namespace Bim.Domain
{
    public interface IWindow
    {
        IDimension Dimensions { get; set; }
        ILocation Location { get; set; }
    }
}