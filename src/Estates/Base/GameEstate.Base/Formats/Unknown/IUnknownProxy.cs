using System.Numerics;

namespace GameEstate.Formats.Unknown
{
    public interface IUnknownProxy
    {
        Vector3[] Vertexs { get; }
        int[] Indexs { get; }
    }
}
