using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class CSectorDataObject : CVariable
    {
        [Ordinal(0), RED] public CUInt8 type { get; set; }
        [Ordinal(1), RED] public CUInt8 flags { get; set; }
        [Ordinal(2), RED] public CUInt16 radius { get; set; }
        [Ordinal(3), RED] public CUInt64 offset { get; set; }
        [Ordinal(4), RED] public CFloat positionX { get; set; }
        [Ordinal(5), RED] public CFloat positionY { get; set; }
        [Ordinal(6), RED] public CFloat positionZ { get; set; }

        public CSectorDataObject(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CSectorDataObject(cr2w, parent, name);

        public override void Read(BinaryReader r, uint size) => base.Read(r, size);

        public override void Write(BinaryWriter w) => base.Write(w);
    }
}