using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.IO;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = ""), REDMeta()]
    public class CMatrix : CVariable
    {
        [Ordinal(1), RED("X")] public Vector X { get; set; }
        [Ordinal(2), RED("Y")] public Vector Y { get; set; }
        [Ordinal(3), RED("Z")] public Vector Z { get; set; }
        [Ordinal(4), RED("W")] public Vector W { get; set; }

        public CMatrix(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CMatrix(cr2w, parent, name);

        public override void Read(BinaryReader r, uint size) => base.Read(r, size);

        public override void Write(BinaryWriter w) => base.Write(w);

    }
}