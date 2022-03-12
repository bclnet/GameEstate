using GameEstate.Red.Formats.Red.CR2W;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Arrays
{
    [REDMeta]
    public class CBufferVLQInt32<T> : CBufferBase<T> where T : CVariable
    {
        public CBufferVLQInt32(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public override void Read(BinaryReader r, uint size) => base.Read(r, size, (int)r.ReadVLQInt32());

        public override void Write(BinaryWriter w)
        {
            if (elements.Count == 0) w.Write((byte)0x80);
            else { var count = new CVLQInt32(cr2w, null, "") { val = elements.Count }; count.Write(w); }
            base.Write(w);
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CBufferVLQInt32<T>(cr2w, parent, name);
    }
}