using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class W3SE_ManageDoor : W3SwitchEvent
	{
		[Ordinal(1)] [RED("doorTag")] 		public CName DoorTag { get; set;}

		[Ordinal(2)] [RED("operations", 2,0)] 		public CArray<CEnum<EDoorOperation>> Operations { get; set;}

		[Ordinal(3)] [RED("force")] 		public CBool Force { get; set;}

		public W3SE_ManageDoor(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new W3SE_ManageDoor(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}