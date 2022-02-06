using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CBTTaskIsFriendlyNearTargetDef : IBehTreeConditionalTaskDefinition
	{
		[Ordinal(1)] [RED("useCombatTarget")] 		public CBool UseCombatTarget { get; set;}

		[Ordinal(2)] [RED("considerOwnerAsFriendly")] 		public CBool ConsiderOwnerAsFriendly { get; set;}

		[Ordinal(3)] [RED("radius")] 		public CFloat Radius { get; set;}

		public CBTTaskIsFriendlyNearTargetDef(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBTTaskIsFriendlyNearTargetDef(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}