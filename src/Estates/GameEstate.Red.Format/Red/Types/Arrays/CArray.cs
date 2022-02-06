using GameEstate.Red.Formats.Red.CR2W;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Arrays
{
    [REDMeta()]
    public class CArray<T> : CArrayBase<T> where T : CVariable
    {
        public CArray(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public override void Read(BinaryReader r, uint size) => base.Read(r, size, (int)r.ReadUInt32());

        public override void Write(BinaryWriter w)
        {
            var count = new CUInt32(cr2w, null, "")
            {
                val = (uint)Elements.Count
            };
            count.Write(w);
            base.Write(w);
        }
    }
}