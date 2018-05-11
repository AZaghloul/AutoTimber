namespace Bim.Domain
{
    public interface ILocation
    {
        float X { get; set; }
        float Y { get; set; }
        float Z { get; set; }
    }
    public class Location : ILocation
    {
        public float X { get; set ; }
        public float Y { get; set ; }
        public float Z { get; set ; }
        public Location(float x, float y, float z)
        {
            X = x;Y = y;Z = z;
        }
    }
}