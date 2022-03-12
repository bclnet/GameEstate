using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs.Complex
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    class SBlockDataDecal : CVariable
    {
        [Ordinal(0), RED] public CUInt16 diffuseTexture { get; set; }
        [Ordinal(1), RED] public CUInt16 padding { get; set; }
        [Ordinal(2), RED] public CUInt32 specularColor { get; set; }
        [Ordinal(3), RED] public CFloat normalTreshold { get; set; }
        //[Ordinal(4), RED] public CFloat specularity { get; set; }
        //[Ordinal(5), RED] public CFloat fadeTime { get; set; }
        [Ordinal(999), REDBuffer(true)] public CBytes unk1 { get; set; }

        public SBlockDataDecal(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            unk1 = new CBytes(cr2w, parent, nameof(unk1));
        }

        public override void Read(BinaryReader r, uint size)
        {
            var startp = r.BaseStream.Position;
            base.Read(r, size);
            var endp = r.BaseStream.Position;
            var read = endp - startp;
            if (read < size) unk1.Read(r, size - (uint)read);
            else if (read > size) throw new FormatException("read too far");
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            if (unk1.Bytes != null && unk1.Bytes.Length > 0) unk1.Write(w);
        }
    }
}
