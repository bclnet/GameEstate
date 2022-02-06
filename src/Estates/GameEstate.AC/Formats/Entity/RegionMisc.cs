using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class RegionMisc : IGetExplorerInfo
    {
        public readonly uint Version;
        public readonly uint GameMapID;
        public readonly uint AutotestMapId;
        public readonly uint AutotestMapSize;
        public readonly uint ClearCellId;
        public readonly uint ClearMonsterId;

        public RegionMisc(BinaryReader r)
        {
            Version = r.ReadUInt32();
            GameMapID = r.ReadUInt32();
            AutotestMapId = r.ReadUInt32();
            AutotestMapSize = r.ReadUInt32();
            ClearCellId = r.ReadUInt32();
            ClearMonsterId = r.ReadUInt32();
        }

        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode($"Version: {Version}"),
                new ExplorerInfoNode($"GameMapID: {GameMapID:X8}"),
                new ExplorerInfoNode($"AutoTest MapID: {AutotestMapId:X8}"),
                new ExplorerInfoNode($"AutoTest MapSize: {AutotestMapSize}"),
                new ExplorerInfoNode($"ClearCellID: {ClearCellId:X8}"),
                new ExplorerInfoNode($"ClearMonsterID: {ClearMonsterId:X8}"),
            };
            return nodes;
        }
    }
}
