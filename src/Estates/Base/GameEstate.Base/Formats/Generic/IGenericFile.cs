using System.Collections.Generic;
using System.Numerics;

namespace GameEstate.Formats.Generic
{
    public enum TextureMap
    {
        Diffuse = 1,
        Bumpmap,
        Specular,
        Environment,
        Decal,
        SubSurface,
        Custom,
        Opacity,
        Detail,
        Heightmap,
        BlendDetail
    }

    public enum ChunkType
    {
        Node = 1,
        Mesh,
        Helper,
    }

    public interface IGenericFile
    {
        string Name { get; }
        string Path { get; }
        IEnumerable<IGenericModel> Models { get; }
        IEnumerable<IGenericMesh> Meshes { get; }
        IEnumerable<IGenericMaterial> Materials { get; }
        IEnumerable<IGenericProxy> Proxies { get; }
        // Collada
        IGenericSkin SkinningInfo { get; }
        IEnumerable<IGenericSource> Sources { get; }
    }

    public interface IGenericModel
    {
        string Path { get; }
    }

    #region Wavefront

    public interface IGenericMesh
    {
        string Name { get; }
        GenericMeshSubset[] Subsets { get; }
        Vector3[] Vertexs { get; }
        int[] Indexs { get; }
        Vector3[] Normals { get; }
        Vector2[] UVs { get; }
        Vector3 MinBound { get; }
        Vector3 MaxBound { get; }
        Vector3 GetTransform(Vector3 vector3);
    }

    public struct GenericMeshSubset
    {
        public int FirstIndex;
        public int NumIndexes;
        public int FirstVertex;
        public int NumVertexs;
        public int MatId;
        public float Radius;
        public Vector3 Center;
    }

    public interface IGenericProxy
    {
        Vector3[] Vertexs { get; }
        int[] Indexs { get; }
    }

    public interface IGenericMaterial
    {
        string Name { get; }
        Vector3? Diffuse { get; } // Color:RGB
        Vector3? Specular { get; } // Color:RGB
        Vector3? Emissive { get; } // Color:RGB
        float Shininess { get; }
        float Opacity { get; }
        IEnumerable<IGenericTexture> Textures { get; }
    }

    public interface IGenericTexture
    {
        string Path { get; }
        TextureMap Map { get; }
    }

    #endregion

    #region Collada

    public struct WORLDTOBONE
    {
    }

    public struct BONETOWORLD
    {
    }

    public interface IGenericSource
    {
        string Author { get; }
        string SourceFile { get; }
    }

    public interface IGenericSkin
    {
        public struct MeshBoneMap
        {
            public int[] BoneIndex;
            public int[] Weight; // Byte / 256?
        }

        public struct IntVertex
        {
            public Vector3 Obsolete0;
            public Vector3 Position;
            public Vector3 Obsolete2;
            public ushort[] BoneIDs; // 4 bone IDs
            public float[] Weights; // Should be 4 of these
            public object Color;
        }

        bool HasSkinningInfo { get; }
        ICollection<IGenericBone> CompiledBones { get; }
        IntVertex[] IntVertexs { get; }
        MeshBoneMap[] BoneMaps { get; }
        ushort[] Ext2IntMaps { get; }
    }

    public interface IGenericBone
    {
        string Name { get; }
        WORLDTOBONE WorldToBone { get; } // 4x3 matrix
        BONETOWORLD BoneToWorld { get; } // 4x3 matrix of world translations/rotations of the bones.
    }

    #endregion
}

//IGenericNode Root { get; }
//IEnumerable<IGenericNode> Nodes { get; }

//public interface IGenericNode
//{
//    string Name { get; }
//    ChunkType Type { get; }
//    IGenericNode Parent { get; }
//    IChunk Object { get; }
//    public IEnumerable<IGenericNode> Children { get; set; }
//}

//public interface IChunk
//{
//    ChunkType Type { get; }
//}

//public interface IChunkMesh : IChunk
//{
//    int Id { get; set; }
//    int MeshSubsets { get; set; }
//    int VerticesData { get; set; }
//    int VertsUVsData { get; set; }
//}