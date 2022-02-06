using GameEstate.Explorer;
using GameEstate.Formats;
using GameEstate.AC.Formats.Props;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class CellPortal : IGetExplorerInfo
    {
        public readonly PortalFlags Flags;
        public readonly ushort PolygonId;
        public readonly ushort OtherCellId;
        public readonly ushort OtherPortalId;
        public bool ExactMatch => (Flags & PortalFlags.ExactMatch) != 0;
        public bool PortalSide => (Flags & PortalFlags.PortalSide) == 0;

        public CellPortal(BinaryReader r)
        {
            Flags = (PortalFlags)r.ReadUInt16();
            PolygonId = r.ReadUInt16();
            OtherCellId = r.ReadUInt16();
            OtherPortalId = r.ReadUInt16();
        }

        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                Flags != 0 ? new ExplorerInfoNode($"Flags: {Flags}") : null,
                new ExplorerInfoNode($"PolygonId: {PolygonId}"),
                OtherCellId != 0 ? new ExplorerInfoNode($"OtherCellId: {OtherCellId:X}") : null,
                OtherPortalId != 0 ? new ExplorerInfoNode($"OtherPortalId: {OtherPortalId:X}") : null,
            };
            return nodes;
        }
    }
}
