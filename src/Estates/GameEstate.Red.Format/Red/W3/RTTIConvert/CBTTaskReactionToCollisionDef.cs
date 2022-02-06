using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CBTTaskReactionToCollisionDef : CBTTaskCollisionMonitorDef
	{
		[Ordinal(1)] [RED("waitTimeout")] 		public CFloat WaitTimeout { get; set;}

		[Ordinal(2)] [RED("activationTimeout")] 		public CFloat ActivationTimeout { get; set;}

		[Ordinal(3)] [RED("knockdownDuration")] 		public CFloat KnockdownDuration { get; set;}

		[Ordinal(4)] [RED("activationScriptEvent")] 		public CName ActivationScriptEvent { get; set;}

		[Ordinal(5)] [RED("deactivateScriptEvent")] 		public CName DeactivateScriptEvent { get; set;}

		public CBTTaskReactionToCollisionDef(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBTTaskReactionToCollisionDef(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}