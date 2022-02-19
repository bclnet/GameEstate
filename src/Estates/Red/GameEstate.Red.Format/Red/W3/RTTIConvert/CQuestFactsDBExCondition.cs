using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CQuestFactsDBExCondition : IQuestCondition
	{
		[Ordinal(1)] [RED("factId1")] 		public CString FactId1 { get; set;}

		[Ordinal(2)] [RED("factId2")] 		public CString FactId2 { get; set;}

		[Ordinal(3)] [RED("queryFact")] 		public CEnum<EQueryFact> QueryFact { get; set;}

		[Ordinal(4)] [RED("compareFunc")] 		public CEnum<ECompareFunc> CompareFunc { get; set;}

		public CQuestFactsDBExCondition(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CQuestFactsDBExCondition(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}