using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CAIBearProtectiveDefaults : CAIBearDefaults
	{
		[Ordinal(1)] [RED("canTaunt")] 		public CBool CanTaunt { get; set;}

		[Ordinal(2)] [RED("berserk")] 		public CBool Berserk { get; set;}

		public CAIBearProtectiveDefaults(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CAIBearProtectiveDefaults(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}