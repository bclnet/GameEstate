using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class HairStyleCG : IGetExplorerInfo
    {
        public readonly uint IconImage;
        public readonly bool Bald;
        public readonly uint AlternateSetup;
        public readonly ObjDesc ObjDesc;

        public HairStyleCG(BinaryReader r)
        {
            IconImage = r.ReadUInt32();
            Bald = r.ReadByte() == 1;
            AlternateSetup = r.ReadUInt32();
            ObjDesc = new ObjDesc(r);
        }

        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                IconImage != 0 ? new ExplorerInfoNode($"Icon: {IconImage:X8}") : null,
                Bald ? new ExplorerInfoNode($"Bald: True") : null,
                AlternateSetup != 0 ? new ExplorerInfoNode($"Alternate Setup: {AlternateSetup:X8}") : null,
                new ExplorerInfoNode("ObjDesc", items: (ObjDesc as IGetExplorerInfo).GetInfoNodes()),
            };
            return nodes;
        }
    }
}
