using GameEstate.Unknown;
using System.Collections.Generic;

namespace GameEstate.Formats.Unknown
{
    public interface IUnknownFileObject
    {
        string Name { get; }
        string Path { get; }
        IEnumerable<IUnknownSource> Sources { get; }
    }
}