using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace GameEstate.AC.Formats.Entity
{
    /// <summary>
    /// Position consists of a CellID + a Frame (Origin + Orientation)
    /// </summary>
    public class Position : IGetExplorerInfo
    {
        public readonly uint ObjCellID;
        public readonly Frame Frame;

        public Position(BinaryReader r)
        {
            ObjCellID = r.ReadUInt32();
            Frame = new Frame(r);
        }

        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                ObjCellID != 0 ? new ExplorerInfoNode($"ObjCellID: {ObjCellID:X8}") : null,
                !Frame.Origin.IsZeroEpsilon() ? new ExplorerInfoNode($"Origin: {Frame.Origin}") : null,
                !Frame.Orientation.IsIdentity ? new ExplorerInfoNode($"Orientation: {Frame.Orientation}") : null,
            };
            return nodes;
        }
    }
}
