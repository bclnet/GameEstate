using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.BufferStructs.Complex;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class SSkeletonRigData : CVariable
    {
        [Ordinal(0), RED] public SVector4D Position { get; set; }
        [Ordinal(1), RED] public SVector4D Rotation { get; set; }
        [Ordinal(2), RED] public SVector4D Scale { get; set; }

        public SSkeletonRigData(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new SSkeletonRigData(cr2w, parent, name);

        public override string ToString() => $"[{Position.ToString()}, {Rotation.ToString()}, {Scale.ToString()}]";
    }
}