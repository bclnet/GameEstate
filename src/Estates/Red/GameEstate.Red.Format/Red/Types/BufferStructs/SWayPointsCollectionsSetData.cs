using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs
{
    [DataContract(Namespace = ""), REDMeta(EREDMetaInfo.REDStruct)]
    public class SWayPointsCollectionsSetData : CVariable
    {
        [Ordinal(0), RED] public CGUID Guid { get; set; }
        [Ordinal(1), RED] public CHandle<CWayPointsCollection> Handle { get; set; }

        public SWayPointsCollectionsSetData(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new SWayPointsCollectionsSetData(cr2w, parent, name);
    }
}