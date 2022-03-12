using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.Runtime.Serialization;
using static GameEstate.Red.Formats.Red.Types.Enums;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = ""), REDMeta]
    public partial class CTerrainTile : CResource
    {
        [Ordinal(1), RED("tileFileVersion")] public CUInt32 TileFileVersion { get; set; }
        [Ordinal(2), RED("collisionType")] public CEnum<ETerrainTileCollision> CollisionType { get; set; }
        [Ordinal(3), RED("maxHeightValue")] public CUInt16 MaxHeightValue { get; set; }
        [Ordinal(4), RED("minHeightValue")] public CUInt16 MinHeightValue { get; set; }

        public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CTerrainTile(cr2w, parent, name);
    }
}