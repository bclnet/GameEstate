using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.IO;
using System.Runtime.Serialization;
using static GameEstate.Red.Formats.Red.Types.Enums;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = ""), REDMeta]
    public partial class CUmbraTile : CResource
    {
        [Ordinal(1), RED("dataStatus")] public CEnum<EUmbraTileDataStatus> DataStatus { get; set; }
        [Ordinal(2), RED("data")] public DeferredDataBuffer Data { get; set; }

        public CUmbraTile(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CUmbraTile(cr2w, parent, name);

        public override void Read(BinaryReader file, uint size) => base.Read(file, size);

        public override void Write(BinaryWriter file) => base.Write(file);
    }
}