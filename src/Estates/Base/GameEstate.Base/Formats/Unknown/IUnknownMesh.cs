using System.Numerics;

namespace GameEstate.Formats.Unknown
{
    public interface IUnknownMesh
    {
        public struct MeshSubset
        {
            public int FirstIndex;
            public int NumIndexes;
            public int FirstVertex;
            public int NumVertexs;
            public int MatId;
            public float Radius;
            public Vector3 Center;
        }

        string Name { get; }
        MeshSubset[] Subsets { get; }
        Vector3[] Vertexs { get; }
        int[] Indexs { get; }
        Vector3[] Normals { get; }
        Vector2[] UVs { get; }
        Vector3 MinBound { get; }
        Vector3 MaxBound { get; }
        Vector3 GetTransform(Vector3 vector3);
    }
}
