using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = ""), REDMeta]
    public partial class CTextureArray : CResource
    {
        [Ordinal(1), RED("bitmaps", 2, 0)] public CArray<CTextureArrayEntry> Bitmaps { get; set; }
        [Ordinal(2), RED("textureGroup")] public CName TextureGroup { get; set; }

        public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CTextureArray(cr2w, parent, name);
    }
}