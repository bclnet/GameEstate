using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CBTTaskLeshyGrabAttackDef : IBehTreeTaskDefinition
	{
		[Ordinal(1)] [RED("attackType")] 		public CEnum<EAttackType> AttackType { get; set;}

		[Ordinal(2)] [RED("stopTaskAfterDealingDmg")] 		public CBool StopTaskAfterDealingDmg { get; set;}

		[Ordinal(3)] [RED("useDirectionalAttacks")] 		public CBool UseDirectionalAttacks { get; set;}

		[Ordinal(4)] [RED("fxOnDamageInstigated")] 		public CName FxOnDamageInstigated { get; set;}

		public CBTTaskLeshyGrabAttackDef(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBTTaskLeshyGrabAttackDef(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}