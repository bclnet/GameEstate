using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = ""), REDMeta]
    public partial class CFont : CResource
    {
        [Ordinal(1), RED("textures", 2, 0)] public CArray<CHandle<CBitmapTexture>> Textures { get; set; }

        public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CFont(cr2w, parent, name);
    }
}