using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    public partial class CFXTrackItem : CFXBase
    {
        [Ordinal(1000), REDBuffer(true)] public CName buffername { get; set; }
        [Ordinal(1001), REDBuffer(true)] public CDynamicInt count { get; set; }
        [Ordinal(1002), REDBuffer(true)] public CUInt8 unk { get; set; }
        [Ordinal(1003), REDBuffer(true)] public CCompressedBuffer<CBufferUInt16<CFloat>> buffer { get; set; }

        public CFXTrackItem(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            buffername = new CName(cr2w, this, nameof(buffername)) { IsSerialized = true };
            count = new CDynamicInt(cr2w, this, nameof(count)) { IsSerialized = true };
            unk = new CUInt8(cr2w, this, nameof(unk)) { IsSerialized = true };
            buffer = new CCompressedBuffer<CBufferUInt16<CFloat>>(cr2w, this, nameof(buffer)) { IsSerialized = true };
        }

        public override void Read(BinaryReader r, uint size)
        {
            var startpos = r.BaseStream.Position;
            base.Read(r, size);
            var endpos = r.BaseStream.Position;
            var bytesread = endpos - startpos;
            if (bytesread < size)
            {
                buffername.Read(r, 2);
                count.Read(r, size);
                unk.Read(r, 1);
                buffer.Read(r, 0, count.val);
            }
            else if (bytesread > size) { }
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            if (buffername != null) buffername.Write(w);
            if (count != null) count.Write(w);
            if (buffername != null) unk.Write(w);
            if (buffer != null) buffer.Write(w);
        }
    }
}