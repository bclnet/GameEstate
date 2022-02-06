using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CPoseCompressionDefaultWithExtraBones : CPoseCompressionDefault
	{
		[Ordinal(1)] [RED("extraRotBones", 2,0)] 		public CArray<CInt32> ExtraRotBones { get; set;}

		[Ordinal(2)] [RED("extraTransBones", 2,0)] 		public CArray<CInt32> ExtraTransBones { get; set;}

		public CPoseCompressionDefaultWithExtraBones(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CPoseCompressionDefaultWithExtraBones(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}