using FastMember;
using GameEstate.Red.Formats.Red.CR2W;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs.Complex
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    class SBlockDataCollisionObject : CVariable
    {
        [Ordinal(0), RED] public CUInt16 meshIndex { get; set; }
        [Ordinal(1), RED] public CUInt16 padding { get; set; }
        [Ordinal(2), RED] public CUInt64 collisionMask { get; set; }
        [Ordinal(3), RED] public CUInt64 collisionGroup { get; set; }

        public SBlockDataCollisionObject(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
    }
}