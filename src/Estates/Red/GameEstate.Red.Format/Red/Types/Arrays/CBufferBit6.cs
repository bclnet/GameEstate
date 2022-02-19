using GameEstate.Red.Formats.Red.CR2W;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Arrays
{
    [REDMeta()]
    public class CBufferBit6<T> : CBufferBase<T> where T : CVariable
    {
        public CBufferBit6(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public override void Read(BinaryReader r, uint size) => base.Read(r, size, (int)r.ReadBit6());

        public override void Write(BinaryWriter w)
        {
            var count = new CDynamicInt(cr2w, null, "")
            {
                val = elements.Count
            };
            count.Write(w);
            base.Write(w);
        }
    }
}