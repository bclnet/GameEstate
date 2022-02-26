﻿using GameEstate.Formats.Unknown;
using System.Collections.Generic;

namespace GameEstate.Cry.Formats.Unknown
{
    public class UnknownFileModel : UnknownFileObject, IUnknownFileModel
    {
        public IEnumerable<IUnknownModel> Models { get; }
        public IEnumerable<IUnknownMesh> Meshes { get; }
        public IEnumerable<IUnknownMaterial> Materials => File.Materials;
        public IEnumerable<IUnknownProxy> Proxies { get; }
        public IUnknownSkin SkinningInfo { get; }
    }
}
