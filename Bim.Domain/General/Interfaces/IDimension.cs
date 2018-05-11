namespace Bim.Domain
{
    public interface IDimension
    {
        float ZDim { get; set; }
        float XDim { get; set; }
        float YDim { get; set; }
    }
    public class Dimension : IDimension
    {
        public float ZDim { get; set; }
        public float XDim { get; set; }
        public float YDim { get; set; }
        public Dimension(float xDim, float yDim, float zDim)
        {
            XDim = xDim; YDim = yDim; ZDim = zDim;
        }
    }
}