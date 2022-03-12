using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = ""), REDMeta]
    public partial class CGenericGrassMask : CResource
    {
        [Ordinal(1), RED("maskRes")] public CUInt32 MaskRes { get; set; }

        public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CGenericGrassMask(cr2w, parent, name);
    }
}