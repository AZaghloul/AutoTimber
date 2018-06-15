
using System.Linq;

namespace Bim.Domain
{
    public interface IVersion
    {
        IModel Model { get; set; }
        string CurrentVersion { get; set; }
        void Upgrade();
        void Downgrade();

    }
}
