using System.IO;
using System.Text;

namespace GameEstate.AC.Formats.Entity
{
    public class Contract
    {
        public readonly uint Version;
        public readonly uint ContractId;
        public readonly string ContractName;

        public readonly string Description;
        public readonly string DescriptionProgress;

        public readonly string NameNPCStart;
        public readonly string NameNPCEnd;

        public readonly string QuestflagStamped;
        public readonly string QuestflagStarted;
        public readonly string QuestflagFinished;
        public readonly string QuestflagProgress;
        public readonly string QuestflagTimer;
        public readonly string QuestflagRepeatTime;

        public readonly Position LocationNPCStart;
        public readonly Position LocationNPCEnd;
        public readonly Position LocationQuestArea;

        public Contract(BinaryReader r)
        {
            Version = r.ReadUInt32();
            ContractId = r.ReadUInt32();
            ContractName = r.ReadL16String(Encoding.Default); r.AlignBoundary();

            Description = r.ReadL16String(Encoding.Default); r.AlignBoundary();
            DescriptionProgress = r.ReadL16String(Encoding.Default); r.AlignBoundary();

            NameNPCStart = r.ReadL16String(Encoding.Default); r.AlignBoundary();
            NameNPCEnd = r.ReadL16String(Encoding.Default); r.AlignBoundary();

            QuestflagStamped = r.ReadL16String(Encoding.Default); r.AlignBoundary();
            QuestflagStarted = r.ReadL16String(Encoding.Default); r.AlignBoundary();
            QuestflagFinished = r.ReadL16String(Encoding.Default); r.AlignBoundary();
            QuestflagProgress = r.ReadL16String(Encoding.Default); r.AlignBoundary();
            QuestflagTimer = r.ReadL16String(Encoding.Default); r.AlignBoundary();
            QuestflagRepeatTime = r.ReadL16String(Encoding.Default); r.AlignBoundary();

            LocationNPCStart = new Position(r);
            LocationNPCEnd = new Position(r);
            LocationQuestArea = new Position(r);
        }
    }
}
