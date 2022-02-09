using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class PhysicsScriptData : IGetExplorerInfo
    {
        public readonly double StartTime;
        public readonly AnimationHook Hook;

        public PhysicsScriptData(BinaryReader r)
        {
            StartTime = r.ReadDouble();
            Hook = AnimationHook.Factory(r);
        }

        //: Entity.PhysicsScriptData
        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode($"StartTime: {StartTime}"),
                new ExplorerInfoNode($"{Hook}"),
            };
            return nodes;
        }
    }
}
