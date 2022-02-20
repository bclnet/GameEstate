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
        IEnumerable<IGenericMesh> Meshes { get; }
        IEnumerable<IGenericMaterial> Materials { get; }
        IEnumerable<IGenericProxy> Proxies { get; }
    }

    public interface IGenericMesh
    {
        string Name { get; }
        GenericMeshSubset[] Subsets { get; }
        Vector3[] Vertices { get; }
        int[] Indices { get; }
        Vector3[] Normals { get; }
        Vector2[] UVs { get; }
        Vector3 MinBound { get; }
        Vector3 MaxBound { get; }
        Vector3 GetTransform(Vector3 vector3);
    }

    public struct GenericMeshSubset
    {
        public int FirstIndex;
        public int NumIndices;
        public int FirstVertex;
        public int NumVertices;
        public int MatId;
        public float Radius;
        public Vector3 Center;
    }

    public interface IGenericProxy
    {
        Vector3[] Vertices { get; }
        int[] Indices { get; }
    }

    public interface IGenericMaterial
    {
        string Name { get; }
        Vector3 Diffuse { get; } // R/G/B
        Vector3 Specular { get; } // R/G/B
        float Shininess { get; }
        float Opacity { get; }
        IEnumerable<IGenericTexture> Textures { get; }
    }

    public interface IGenericTexture
    {
        string Path { get; }
        TextureMap Map { get; }
    }
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