using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class W3Reputation : CObject
	{
		[Ordinal(1)] [RED("factionReputations", 2,0)] 		public CArray<CHandle<W3FactionReputationPoints>> FactionReputations { get; set;}

		[Ordinal(2)] [RED("factionName")] 		public CEnum<EFactionName> FactionName { get; set;}

		public W3Reputation(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new W3Reputation(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}