using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.AC.Formats.FileTypes
{
    /// <summary>
    /// Reads and stores the XP Tables from the client_portal.dat (file 0x0E000018).
    /// </summary>
    [PakFileType(PakFileType.XpTable)]
    public class XpTable : FileType, IGetExplorerInfo
    {
        public const uint FILE_ID = 0x0E000018;

        public readonly uint[] AttributeXpList;
        public readonly uint[] VitalXpList;
        public readonly uint[] TrainedSkillXpList;
        public readonly uint[] SpecializedSkillXpList;
        /// <summary>
        /// The XP needed to reach each level
        /// </summary>
        public readonly ulong[] CharacterLevelXPList;
        /// <summary>
        /// Number of credits gifted at each level
        /// </summary>
        public readonly uint[] CharacterLevelSkillCreditList;

        public XpTable(BinaryReader r)
        {
            Id = r.ReadUInt32();
            // The counts for each "Table" are at the top of the file.
            var attributeCount = r.ReadInt32();
            var vitalCount = r.ReadInt32();
            var trainedSkillCount = r.ReadInt32();
            var specializedSkillCount = r.ReadInt32();
            var levelCount = r.ReadUInt32();
            AttributeXpList = r.ReadTArray<uint>(sizeof(uint), attributeCount); //: counts are +1?
            VitalXpList = r.ReadTArray<uint>(sizeof(uint), vitalCount); //: counts are +1?
            TrainedSkillXpList = r.ReadTArray<uint>(sizeof(uint), trainedSkillCount); //: counts are +1?
            SpecializedSkillXpList = r.ReadTArray<uint>(sizeof(uint), specializedSkillCount); //: counts are +1?
            CharacterLevelXPList = r.ReadTArray<ulong>(sizeof(ulong), (int)levelCount); //: counts are +1?
            CharacterLevelSkillCreditList = r.ReadTArray<uint>(sizeof(uint), (int)levelCount); //: counts are +1?
        }

        //: New
        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode($"{nameof(XpTable)}: {Id:X8}", items: new List<ExplorerInfoNode> {
                })
            };
            return nodes;
        }
    }
}
