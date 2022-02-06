using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    // TODO: refactor to use existing PaletteOverride object
    public class SubPalette : IGetExplorerInfo
    {
        public readonly uint SubID;
        public readonly uint Offset;
        public readonly uint NumColors;

        public SubPalette(BinaryReader r)
        {
            SubID = r.ReadAsDataIDOfKnownType(0x04000000);
            Offset = (uint)(r.ReadByte() * 8);
            NumColors = r.ReadByte();
            if (NumColors == 0)
                NumColors = 256;
            NumColors *= 8;
        }

        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode($"SubID: {SubID:X8}"),
                new ExplorerInfoNode($"Offset: {Offset:X8}"),
                new ExplorerInfoNode($"NumColors: {NumColors}"),
            };
            return nodes;
        }
    }
}
