using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CBTCondHorsePerformingActionDef : IBehTreeHorseConditionalTaskDefinition
	{
		[Ordinal(1)] [RED("mounting")] 		public CBool Mounting { get; set;}

		[Ordinal(2)] [RED("dismounting")] 		public CBool Dismounting { get; set;}

		[Ordinal(3)] [RED("inAir")] 		public CBool InAir { get; set;}

		public CBTCondHorsePerformingActionDef(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBTCondHorsePerformingActionDef(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}