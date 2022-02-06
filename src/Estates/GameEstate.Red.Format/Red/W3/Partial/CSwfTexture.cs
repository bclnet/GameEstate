using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public partial class CSwfTexture : CBitmapTexture
	{
		[Ordinal(1)] [RED("linkageName")] 		public CString LinkageName { get; set;}

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CSwfTexture(cr2w, parent, name);

	}
}