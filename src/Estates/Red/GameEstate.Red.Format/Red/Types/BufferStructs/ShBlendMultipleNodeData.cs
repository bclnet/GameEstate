using FastMember;
using GameEstate.Red.Formats.Red.CR2W;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class ShBlendMultipleNodeData : CVariable
    {
        [Ordinal(0), RED] public CUInt32 Index { get; set; }
        [Ordinal(1), RED] public CFloat Blendvalue { get; set; }

        public ShBlendMultipleNodeData(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new ShBlendMultipleNodeData(cr2w, parent, name);
    }
}