using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.BufferStructs.Complex;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class SUmbraSceneData : CVariable
    {
        [Ordinal(1000), REDBuffer] public SVector4D Position { get; set; }
        [Ordinal(1001), REDBuffer] public CHandle<CUmbraTile> Umbratile { get; set; }

        public SUmbraSceneData(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new SUmbraSceneData(cr2w, parent, name);
    }
}