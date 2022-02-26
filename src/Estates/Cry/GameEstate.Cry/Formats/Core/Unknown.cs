using GameEstate.Formats.Unknown;
using System;
using System.Collections.Generic;
using NVector2 = System.Numerics.Vector2;
using NVector3 = System.Numerics.Vector3;

namespace GameEstate.Cry.Formats.Core
{
    partial class Material : IUnknownMaterial
    {
        partial class Color
        {
            bool _hasVector3; NVector3 _vector3;
            public ref NVector3 Vector3
            {
                get
                {
                    if (_hasVector3) return ref _vector3;
                    _vector3 = new NVector3((float)Red, (float)Blue, (float)Green);
                    _hasVector3 = true;
                    return ref _vector3;
                }
            }
        }

        partial class Texture : IUnknownTexture
        {
            string IUnknownTexture.Path => File;
            IUnknownTexture.Map IUnknownTexture.Maps => Map switch
            {
                MapTypeEnum.Unknown => 0,
                MapTypeEnum.Diffuse => IUnknownTexture.Map.Diffuse,
                MapTypeEnum.Bumpmap => IUnknownTexture.Map.Bumpmap,
                MapTypeEnum.Specular => IUnknownTexture.Map.Specular,
                MapTypeEnum.Environment => IUnknownTexture.Map.Environment,
                MapTypeEnum.Decal => IUnknownTexture.Map.Decal,
                MapTypeEnum.SubSurface => IUnknownTexture.Map.SubSurface,
                MapTypeEnum.Opacity => IUnknownTexture.Map.Opacity,
                MapTypeEnum.Detail => IUnknownTexture.Map.Detail,
                MapTypeEnum.Heightmap => IUnknownTexture.Map.Heightmap,
                MapTypeEnum.BlendDetail => IUnknownTexture.Map.BlendDetail,
                MapTypeEnum.Custom => IUnknownTexture.Map.Custom,
                _ => throw new ArgumentOutOfRangeException(nameof(Map), $"{Map}")
            };
        }

        string IUnknownMaterial.Name => Name;
        NVector3? IUnknownMaterial.Diffuse => Diffuse?.Vector3;
        NVector3? IUnknownMaterial.Specular => Specular?.Vector3;
        NVector3? IUnknownMaterial.Emissive => Emissive?.Vector3;
        float IUnknownMaterial.Shininess => (float)Shininess;
        float IUnknownMaterial.Opacity => (float)Opacity;
        IEnumerable<IUnknownTexture> IUnknownMaterial.Textures => Textures;
    }

    namespace Chunks
    {
        partial class ChunkMesh : IUnknownMesh
        {
            string IUnknownMesh.Name => throw new NotImplementedException();
            IUnknownMesh.MeshSubset[] IUnknownMesh.Subsets => throw new NotImplementedException();
            NVector3[] IUnknownMesh.Vertexs => throw new NotImplementedException();
            int[] IUnknownMesh.Indexs => throw new NotImplementedException();
            NVector3[] IUnknownMesh.Normals => throw new NotImplementedException();
            NVector2[] IUnknownMesh.UVs => throw new NotImplementedException();
            NVector3 IUnknownMesh.MinBound => throw new NotImplementedException();
            NVector3 IUnknownMesh.MaxBound => throw new NotImplementedException();
            NVector3 IUnknownMesh.GetTransform(NVector3 vector3)
            {
                throw new NotImplementedException();
            }
        }
    }
}
