using Bim.Common.Measures;

namespace Bim.Domain
{
    public interface IDimension
    {
        Length ZDim { get; set; }
        Length XDim { get; set; }
        Length YDim { get; set; }
    }
   
}