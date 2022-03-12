using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.IO;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = ""), REDMeta]
    public partial class CMaterialInstance : IMaterial
    {
        [Ordinal(1), RED("baseMaterial")] public CHandle<IMaterial> BaseMaterial { get; set; }
        [Ordinal(2), RED("enableMask")] public CBool EnableMask { get; set; }

        public CMaterialInstance(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CMaterialInstance(cr2w, parent, name);

        public override void Read(BinaryReader file, uint size) => base.Read(file, size);

        public override void Write(BinaryWriter file) => base.Write(file);
    }
}