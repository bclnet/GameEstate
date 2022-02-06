using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class W3SE_ManageGate : W3SwitchEvent
	{
		[Ordinal(1)] [RED("entityTag")] 		public CName EntityTag { get; set;}

		[Ordinal(2)] [RED("open")] 		public CBool Open { get; set;}

		[Ordinal(3)] [RED("speedModifier")] 		public CFloat SpeedModifier { get; set;}

		public W3SE_ManageGate(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new W3SE_ManageGate(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}