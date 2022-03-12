using GameEstate.Red.Formats.Red.CR2W;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Arrays
{
    [REDMeta]
    public class CBufferUInt16<T> : CBufferBase<T> where T : CVariable
    {
        public CBufferUInt16(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBufferUInt16<T>(cr2w, parent, name);

        public override void Read(BinaryReader r, uint size)
        {
            var count = new CUInt16(cr2w, null, "");
            count.Read(r, size);
            base.Read(r, size, (int)count.val);
        }

        public override void Write(BinaryWriter w)
        {
            var count = new CUInt16(cr2w, null, "");
            count.val = (ushort)elements.Count;
            count.Write(w);
            base.Write(w);
        }
    }
}