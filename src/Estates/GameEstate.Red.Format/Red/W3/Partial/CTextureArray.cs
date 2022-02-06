using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public partial class CTextureArray : CResource
	{
		[Ordinal(1)] [RED("bitmaps", 2,0)] 		public CArray<CTextureArrayEntry> Bitmaps { get; set;}

		[Ordinal(2)] [RED("textureGroup")] 		public CName TextureGroup { get; set;}

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CTextureArray(cr2w, parent, name);

	}
}