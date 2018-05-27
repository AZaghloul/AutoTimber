
using System.Linq;

namespace Bim.Common
{
    public interface IVersion
    {
        IModel IfModel { get; set; }
        string CurrentVersion { get; set; }
        void Upgrade();
        void Downgrade();

    }
}
