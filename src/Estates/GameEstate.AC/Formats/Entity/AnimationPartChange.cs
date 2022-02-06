using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class AnimationPartChange : IGetExplorerInfo
    {
        public readonly byte PartIndex;
        public readonly uint PartID;

        public AnimationPartChange(BinaryReader r)
        {
            PartIndex = r.ReadByte();
            PartID = r.ReadAsDataIDOfKnownType(0x01000000);
        }
        public AnimationPartChange(BinaryReader r, ushort partIndex)
        {
            PartIndex = (byte)(partIndex & 255);
            PartID = r.ReadAsDataIDOfKnownType(0x01000000);
        }

        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode($"PartIdx: {PartIndex}"),
                new ExplorerInfoNode($"PartID: {PartID:X8}"),
            };
            return nodes;
        }

        public override string ToString() => $"PartIdx: {PartIndex}, PartID: {PartID:X8}";
    }
}
