using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CBTTaskDisableHitReactionDef : IBehTreeTaskDefinition
	{
		[Ordinal(1)] [RED("onActivate")] 		public CBool OnActivate { get; set;}

		[Ordinal(2)] [RED("onDeactivate")] 		public CBool OnDeactivate { get; set;}

		[Ordinal(3)] [RED("overrideForThisTask")] 		public CBool OverrideForThisTask { get; set;}

		public CBTTaskDisableHitReactionDef(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBTTaskDisableHitReactionDef(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}