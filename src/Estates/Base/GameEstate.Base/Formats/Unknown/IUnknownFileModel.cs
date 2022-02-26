using System.Collections.Generic;

namespace GameEstate.Formats.Unknown
{
    public interface IUnknownFileModel : IUnknownFileObject
    {
        IEnumerable<IUnknownModel> Models { get; }
        IEnumerable<IUnknownMesh> Meshes { get; }
        IEnumerable<IUnknownMaterial> Materials { get; }
        IEnumerable<IUnknownProxy> Proxies { get; }
        IUnknownSkin SkinningInfo { get; }
    }
}
