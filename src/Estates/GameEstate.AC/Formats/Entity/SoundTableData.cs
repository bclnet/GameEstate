using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class SoundTableData : IGetExplorerInfo
    {
        public readonly uint SoundId; // Corresponds to the DatFileType.Wave
        public readonly float Priority;
        public readonly float Probability;
        public readonly float Volume;

        public SoundTableData(BinaryReader r)
        {
            SoundId = r.ReadUInt32();
            Priority = r.ReadSingle();
            Probability = r.ReadSingle();
            Volume = r.ReadSingle();
        }

        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode($"Sound ID: {SoundId:X8}"),
                new ExplorerInfoNode($"Priority: {Priority}"),
                new ExplorerInfoNode($"Probability: {Probability}"),
                new ExplorerInfoNode($"Volume: {Volume}"),
            };
            return nodes;
        }
    }
}
