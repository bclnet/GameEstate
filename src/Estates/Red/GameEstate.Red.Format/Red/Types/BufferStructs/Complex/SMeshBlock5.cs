using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs.Complex
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class SMeshBlock5 : CVariable
    {
        const int fixedbuffersize = 46;

        [Ordinal(0), RED] public CUInt16 bytesize { get; set; }
        [Ordinal(1), RED] public CBytes unk1 { get; set; }

        public SMeshBlock5(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            bytesize = new CUInt16(cr2w, this, nameof(bytesize)) { IsSerialized = true };
            unk1 = new CBytes(cr2w, this, nameof(unk1)) { IsSerialized = true };
        }

        public override void Read(BinaryReader r, uint size)
        {
            bytesize.Read(r, 2);
            if (bytesize.val != fixedbuffersize) throw new NotImplementedException();
            unk1.Read(r, (uint)bytesize.val - 2);
        }

        public override void Write(BinaryWriter w)
        {
            bytesize.Write(w);
            unk1.Write(w);
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new SMeshBlock5(cr2w, parent, name);

        public override string ToString() => "";
    }
}