using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class CloSubPaletteRange : IGetExplorerInfo
    {
        public readonly uint Offset;
        public readonly uint NumColors;

        public CloSubPaletteRange(BinaryReader r)
        {
            Offset = r.ReadUInt32();
            NumColors = r.ReadUInt32();
        }

        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode($"Offset: {Offset}"),
                new ExplorerInfoNode($"NumColors: {NumColors}"),
            };
            return nodes;
        }

        public override string ToString() => $"Offset: {Offset}, NumColors: {NumColors}";
    }
}
