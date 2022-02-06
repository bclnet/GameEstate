using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class AnimData : IGetExplorerInfo
    {
        public readonly uint AnimId;
        public readonly int LowFrame;
        public readonly int HighFrame;
        /// <summary>
        /// Negative framerates play animation in reverse
        /// </summary>
        public readonly float Framerate;

        public AnimData(BinaryReader r)
        {
            AnimId = r.ReadUInt32();
            LowFrame = r.ReadInt32();
            HighFrame = r.ReadInt32();
            Framerate = r.ReadSingle();
        }

        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode($"AnimId: {AnimId:X8}"),
                new ExplorerInfoNode($"Low frame: {LowFrame}"),
                new ExplorerInfoNode($"High frame: {HighFrame}"),
                new ExplorerInfoNode($"Framerate: {Framerate}"),
            };
            return nodes;
        }

        public override string ToString() => $"AnimId: {AnimId:X8}, LowFrame: {LowFrame}, HighFrame: {HighFrame}, FrameRate: {Framerate}";
    }
}
