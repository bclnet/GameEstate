using System.IO;
using System.Runtime.Serialization;
using GameEstate.Red.Formats.Red.CR2W.Reflection;
using FastMember;
using static GameEstate.Red.Formats.Red.Records.Enums;


namespace GameEstate.Red.Formats.Red.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public partial class CStorySceneScript : CStorySceneControlPart
	{
		[Ordinal(1)] [RED("functionName")] 		public CName FunctionName { get; set;}

		[Ordinal(2)] [RED("links", 2,0)] 		public CArray<CPtr<CStorySceneLinkElement>> Links { get; set;}

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CStorySceneScript(cr2w, parent, name);

	}
}