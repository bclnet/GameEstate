using GameEstate.Explorer;
using GameEstate.AC.Formats.Props;
using System.Collections.Generic;
using System.IO;
using GameEstate.Formats;

namespace GameEstate.AC.Formats.Entity
{
    public class SkillCG : IGetExplorerInfo
    {
        public readonly Skill SkillNum;
        public readonly int NormalCost;
        public readonly int PrimaryCost;

        public SkillCG(BinaryReader r)
        {
            SkillNum = (Skill)r.ReadUInt32();
            NormalCost = r.ReadInt32();
            PrimaryCost = r.ReadInt32();
        }

        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode($"Skill: {SkillNum}"),
                new ExplorerInfoNode($"Normal Cost: {NormalCost}"),
                new ExplorerInfoNode($"Primary Cost: {PrimaryCost}"),
            };
            return nodes;
        }
    }
}
