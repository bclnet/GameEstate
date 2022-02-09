using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameEstate.AC.Formats.Entity
{
    public class PhysicsScriptTableData : IGetExplorerInfo
    {
        public readonly ScriptAndModData[] Scripts;

        public PhysicsScriptTableData(BinaryReader r)
            => Scripts = r.ReadL32Array(x => new ScriptAndModData(r));

        //: Entity.PhysicsScriptTableData
        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode("ScriptMods", items: Scripts.Select(x=>new ExplorerInfoNode($"{x}"))),
            };
            return nodes;
        }
    }
}
