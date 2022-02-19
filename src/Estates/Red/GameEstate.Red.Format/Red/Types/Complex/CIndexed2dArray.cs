using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using System.IO;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    [DataContract(Namespace = ""), REDMeta]
    class CIndexed2dArray : CVariable
    {
        [Ordinal(0), RED("headers", 12, 0)] public CArray<CString> Headers { get; set; }
        [Ordinal(1), RED("data", 12, 0, 12, 0)] public CArray<CArray<CString>> Data { get; set; }
        [Ordinal(1001), REDBuffer] public CBufferVLQInt32<CString> Strings1 { get; set; }
        [Ordinal(1002), REDBuffer] public CBufferVLQInt32<CBufferVLQInt32<CString>> Strings2 { get; set; }

        public CIndexed2dArray(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public override void Read(BinaryReader r, uint size) => base.Read(r, size);

        public override void Write(BinaryWriter w) => base.Write(w);
    }
}
