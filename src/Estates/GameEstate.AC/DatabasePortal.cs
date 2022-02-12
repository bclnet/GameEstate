using GameEstate.AC.Formats.FileTypes;

namespace GameEstate.AC
{
    public class DatabasePortal : Database
    {
        public DatabasePortal(EstatePakFile pakFile) : base(pakFile)
        {
            BadData = ReadFile<BadData>(BadData.FILE_ID);
            ChatPoseTable = ReadFile<ChatPoseTable>(ChatPoseTable.FILE_ID);
            CharGen = ReadFile<CharGen>(CharGen.FILE_ID);
            ContractTable = ReadFile<ContractTable>(ContractTable.FILE_ID);
            GeneratorTable = ReadFile<GeneratorTable>(GeneratorTable.FILE_ID);
            NameFilterTable = ReadFile<NameFilterTable>(NameFilterTable.FILE_ID);
            RegionDesc = ReadFile<RegionDesc>(RegionDesc.FILE_ID);
            SecondaryAttributeTable = ReadFile<SecondaryAttributeTable>(SecondaryAttributeTable.FILE_ID);
            SkillTable = ReadFile<SkillTable>(SkillTable.FILE_ID);
            SpellComponentTable = ReadFile<SpellComponentTable>(SpellComponentTable.FILE_ID);
            SpellTable = ReadFile<SpellTable>(SpellTable.FILE_ID);
            TabooTable = ReadFile<TabooTable>(TabooTable.FILE_ID);
            XpTable = ReadFile<XpTable>(XpTable.FILE_ID);
        }

        public BadData BadData { get; }
        public ChatPoseTable ChatPoseTable { get; }
        public CharGen CharGen { get; }
        public ContractTable ContractTable { get; }
        public GeneratorTable GeneratorTable { get; }
        public NameFilterTable NameFilterTable { get; }
        public RegionDesc RegionDesc { get; }
        public SecondaryAttributeTable SecondaryAttributeTable { get; }
        public SkillTable SkillTable { get; }
        public SpellComponentTable SpellComponentTable { get; }
        public SpellTable SpellTable { get; }
        public TabooTable TabooTable { get; }
        public XpTable XpTable { get; }
    }
}
