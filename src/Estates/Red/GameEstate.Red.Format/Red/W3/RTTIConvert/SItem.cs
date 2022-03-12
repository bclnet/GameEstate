using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.IO;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [DataContract(Namespace = "")]
    [REDMeta]
    public class SItem : CVariable
    {
        [Ordinal(1)][RED("itemName")] public CName ItemName { get; set; }

        [Ordinal(2)][RED("quantity")] public CInt32 Quantity { get; set; }

        public SItem(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new SItem(cr2w, parent, name);

        public override void Read(BinaryReader file, uint size) => base.Read(file, size);

        public override void Write(BinaryWriter file) => base.Write(file);

    }
}