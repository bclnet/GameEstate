using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CJournalBase : CObject
	{
		[Ordinal(1)] [RED("guid")] 		public CGUID Guid { get; set;}

		[Ordinal(2)] [RED("baseName")] 		public CString BaseName { get; set;}

		[Ordinal(3)] [RED("order")] 		public CUInt32 Order { get; set;}

		[Ordinal(4)] [RED("uniqueScriptIdentifier")] 		public CName UniqueScriptIdentifier { get; set;}

		public CJournalBase(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CJournalBase(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}