using FastMember;
using GameEstate.Red.Formats.Red.CR2W;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class SAnimPointCloudLookAtParamData : CVariable
    {
        [Ordinal(0), RED] public CUInt16 unk1 { get; set; }
        [Ordinal(1), RED] public CUInt16 unk2 { get; set; }
        [Ordinal(2), RED] public CUInt16 unk3 { get; set; }

        public SAnimPointCloudLookAtParamData(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new SAnimPointCloudLookAtParamData(cr2w, parent, name);

        public override string ToString() => $"[{unk1.ToString()}, {unk2.ToString()}, {unk3.ToString()}]";
    }
}