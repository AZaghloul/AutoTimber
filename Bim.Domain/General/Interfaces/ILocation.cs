
using Bim.Common.Measures;

namespace Bim.Domain
{
    public interface ILocation
    {
        Length X { get; set; }
        Length Y { get; set; }
        Length Z { get; set; }
    }
   
}