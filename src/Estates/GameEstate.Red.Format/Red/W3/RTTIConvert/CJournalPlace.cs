using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CJournalPlace : CJournalContainer
	{
		[Ordinal(1)] [RED("name")] 		public LocalizedString Name { get; set;}

		[Ordinal(2)] [RED("image")] 		public CString Image { get; set;}

		[Ordinal(3)] [RED("active")] 		public CBool Active { get; set;}

		public CJournalPlace(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CJournalPlace(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}