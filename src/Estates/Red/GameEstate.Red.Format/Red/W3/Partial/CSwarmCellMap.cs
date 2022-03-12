using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = ""), REDMeta]
    public partial class CSwarmCellMap : CResource
    {
        [Ordinal(1), RED("cellSize")] public CFloat CellSize { get; set; }

        public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CSwarmCellMap(cr2w, parent, name);
    }
}