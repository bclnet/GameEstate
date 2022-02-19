using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class SMipData : CVariable
    {
        [Ordinal(0), RED] public CUInt32 Width { get; set; }
        [Ordinal(1), RED] public CUInt32 Height { get; set; }
        [Ordinal(2), RED] public CUInt32 Blocksize { get; set; }
        [Ordinal(1004), REDBuffer(true)] public CByteArray Mip { get; set; }

        public SMipData(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            Mip = new CByteArray(cr2w, this, nameof(Mip)) { IsSerialized = true };
        }

        public override void Read(BinaryReader r, uint size)
        {
            base.Read(r, size);
            // if is uncooked
            if (REDFlags == 0)
                Mip.Read(r, 0);
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            // if is uncooked
            if (REDFlags == 0)
                Mip.Write(w);
        }
    }
}