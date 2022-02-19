using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public partial class CFXTrackItem : CFXBase
	{
		[Ordinal(1)] [RED("timeBegin")] 		public CFloat TimeBegin { get; set;}

		[Ordinal(2)] [RED("timeDuration")] 		public CFloat TimeDuration { get; set;}

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CFXTrackItem(cr2w, parent, name);

	}
}