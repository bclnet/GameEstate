using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = ""), REDMeta]
    public partial class CStorySceneScript : CStorySceneControlPart
    {
        [Ordinal(1), RED("functionName")] public CName FunctionName { get; set; }
        [Ordinal(2), RED("links", 2, 0)] public CArray<CPtr<CStorySceneLinkElement>> Links { get; set; }

        public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CStorySceneScript(cr2w, parent, name);
    }
}