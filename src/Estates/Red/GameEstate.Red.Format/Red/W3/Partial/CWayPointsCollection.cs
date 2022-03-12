using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = ""), REDMeta]
    public partial class CWayPointsCollection : CResource
    {
        [Ordinal(1), RED("version")] public CUInt16 Version { get; set; }
        [Ordinal(2), RED("waypointsCount")] public CUInt16 WaypointsCount { get; set; }
        [Ordinal(3), RED("componentsMappingsCount")] public CUInt16 ComponentsMappingsCount { get; set; }
        [Ordinal(4), RED("waypointsGroupsCount")] public CUInt16 WaypointsGroupsCount { get; set; }
        [Ordinal(5), RED("indexesCount")] public CUInt32 IndexesCount { get; set; }
        [Ordinal(6), RED("parties", 2, 0)] public CArray<SPartySpawner> Parties { get; set; }
        [Ordinal(7), RED("partyWaypoints", 2, 0)] public CArray<SPartyWaypointData> PartyWaypoints { get; set; }

        public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CWayPointsCollection(cr2w, parent, name);
    }
}