using FastMember;
using GameEstate.Red.Formats.Red.CR2W;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class SMaterialGraphParameter : CVariable
    {
        [Ordinal(0),RED] public CUInt8 Type { get; set; }
        [Ordinal(1),RED] public CUInt8 Offset { get; set; }
        [Ordinal(2),RED] public CName Name { get; set; }

        public SMaterialGraphParameter(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new SMaterialGraphParameter(cr2w, parent, name);

        public override string ToString() => "";
    }
}