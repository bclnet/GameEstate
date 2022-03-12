using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs
{
    [DataContract(Namespace = ""), REDMeta(EREDMetaInfo.REDStruct)]
    public class SFoliageResourceData : CVariable
    {
        [Ordinal(1000), REDBuffer] public CHandle<CSRTBaseTree> Treetype { get; set; }
        [Ordinal(1001), REDBuffer] public CBufferVLQInt32<SFoliageInstanceData> TreeCollection { get; set; }

        public SFoliageResourceData(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new SFoliageResourceData(cr2w, parent, name);
    }
}