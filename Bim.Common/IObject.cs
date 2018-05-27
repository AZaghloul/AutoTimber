using System;

namespace Bim.Common
{
    public interface IObject
    {
        int Id { get; set; }
        int Label { get; set; }
        string Name { get; set; }
        Guid Guid { get; set; }
        string Description { get; set; }
    }
}