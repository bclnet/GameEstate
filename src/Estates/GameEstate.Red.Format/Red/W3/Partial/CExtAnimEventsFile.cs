using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public partial class CExtAnimEventsFile : CResource
	{
		[Ordinal(1)] [RED("requiredSfxTag")] 		public CName RequiredSfxTag { get; set;}

		public CExtAnimEventsFile(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CExtAnimEventsFile(cr2w, parent, name);

		

	}
}