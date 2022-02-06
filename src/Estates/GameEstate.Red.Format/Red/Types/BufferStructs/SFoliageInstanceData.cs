using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs
{
    [DataContract(Namespace = ""), REDMeta(EREDMetaInfo.REDStruct)]
    public class SFoliageInstanceData : CVariable
    {
        [Ordinal(0), RED] public CFloat PositionX { get; set; }
        [Ordinal(1), RED] public CFloat PositionY { get; set; }
        [Ordinal(2), RED] public CFloat PositionZ { get; set; }
        [Ordinal(3), RED] public CFloat Yaw { get; set; }
        [Ordinal(4), RED] public CFloat Pitch { get; set; }
        [Ordinal(5), RED] public CFloat Roll { get; set; }

        public SFoliageInstanceData(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new SFoliageInstanceData(cr2w, parent, name);
    }
}