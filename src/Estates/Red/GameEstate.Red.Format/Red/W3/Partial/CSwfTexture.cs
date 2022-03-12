using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = ""), REDMeta]
    public partial class CSwfTexture : CBitmapTexture
    {
        [Ordinal(1), RED("linkageName")] public CString LinkageName { get; set; }

        public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CSwfTexture(cr2w, parent, name);
    }
}