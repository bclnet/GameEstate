using GameEstate.Explorer;
using GameEstate.AC.Formats.Props;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameEstate.Formats;

namespace GameEstate.AC.Formats.Entity
{
    public class Polygon : IGetExplorerInfo
    {
        public readonly byte NumPts;
        public readonly StipplingType Stippling; // Whether it has that textured/bumpiness to it
        public readonly CullMode SidesType;
        public readonly short PosSurface;
        public readonly short NegSurface;
        public readonly short[] VertexIds;
        public readonly byte[] PosUVIndices;
        public readonly byte[] NegUVIndices;
        public SWVertex[] Vertices;

        public Polygon(BinaryReader r)
        {
            NumPts = r.ReadByte();
            Stippling = (StipplingType)r.ReadByte();
            SidesType = (CullMode)r.ReadInt32();
            PosSurface = r.ReadInt16();
            NegSurface = r.ReadInt16();
            VertexIds = r.ReadTArray<short>(sizeof(short), NumPts);
            if (!Stippling.HasFlag(StipplingType.NoPos))
                PosUVIndices = r.ReadBytes(NumPts);
            if (SidesType == CullMode.Clockwise && !Stippling.HasFlag(StipplingType.NoNeg))
                NegUVIndices = r.ReadBytes(NumPts);
            if (SidesType == CullMode.None)
            {
                NegSurface = PosSurface;
                NegUVIndices = PosUVIndices;
            }
        }

        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                //new ExplorerInfoNode($"NumPoints: {NumPts}"),
                new ExplorerInfoNode($"Stippling: {Stippling}"),
                new ExplorerInfoNode($"CullMode: {SidesType}"),
                new ExplorerInfoNode($"PosSurface: {PosSurface}"),
                new ExplorerInfoNode($"NegSurface: {NegSurface}"),
                new ExplorerInfoNode($"Vertex IDs: {string.Join(", ", VertexIds)}"),
                new ExplorerInfoNode($"PosUVIndices: {string.Join(", ", PosUVIndices)}"),
                new ExplorerInfoNode($"NegUVIndices: {string.Join(", ", NegUVIndices)}"),
            };
            return nodes;
        }

        public void LoadVertices(CVertexArray vertexArray) => Vertices = VertexIds.Select(id => vertexArray.Vertices[(ushort)id]).ToArray();

    }
}
