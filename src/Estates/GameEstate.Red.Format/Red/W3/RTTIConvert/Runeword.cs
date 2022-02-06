using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class Runeword : CVariable
	{
		[Ordinal(1)] [RED("wordName")] 		public CName WordName { get; set;}

		[Ordinal(2)] [RED("runes", 2,0)] 		public CArray<CName> Runes { get; set;}

		[Ordinal(3)] [RED("abilities", 2,0)] 		public CArray<CName> Abilities { get; set;}

		public Runeword(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new Runeword(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}