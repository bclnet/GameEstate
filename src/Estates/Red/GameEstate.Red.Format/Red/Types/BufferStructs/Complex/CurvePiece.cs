using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System;
using System.Diagnostics;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs.Complex
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class CurvePiece : CVariable
    {
        [Ordinal(0)] [RED] public CUInt16 valueCount { get; set; }
        [Ordinal(1)] [RED] public CCompressedBuffer<CFloat> values { get; set; }

        public CurvePiece(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            // This has a fixed size in memory, but for some reason file format is allowed to not provide all,
            // leaving the rest to zero values. Possibly has individual fields instead of an array.
            values = new CCompressedBuffer<CFloat>(cr2w, this, nameof(values)) { IsSerialized = true };
            valueCount = new CUInt16(cr2w, this, "count") { val = 16, IsSerialized = true };
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var copy = base.Copy(context) as CurvePiece;
            copy.valueCount = valueCount.Copy(context) as CUInt16;
            copy.values = values.Copy(context) as CCompressedBuffer<CFloat>;
            return copy;
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CurvePiece(cr2w, parent, name);

        public override void Read(BinaryReader r, uint size)
        {
            valueCount.Read(r, size);
            if (valueCount.val > 16)
                Debug.Print($"Read: curve piece value count {valueCount.val} exceeds limit {values.Count}");
            values.Read(r, size, valueCount.val);
        }

        public override void Write(BinaryWriter w)
        {
            var writtenCount = Math.Min(valueCount.val, (ushort)values.Count);
            if (writtenCount != valueCount.val)
                Debug.Print($"Write: curve piece value count {valueCount.val} exceeds limit {values.Count}");
            w.Write(writtenCount);
            values.Write(w);
        }
    }
}