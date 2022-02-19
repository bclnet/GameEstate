using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    [REDMeta]
    public class CPhysicalCollision : CVariable
    {
        [Ordinal(1000), REDBuffer(true)] public CUInt32 Unk1 { get; set; }
        [Ordinal(1001), REDBuffer(true)] public CBufferVLQInt32<CName> Collisiontypes { get; set; }
        [Ordinal(1002), REDBuffer(true)] public CBytes Data { get; set; }

        public CPhysicalCollision(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) 
        {
            Unk1 = new CUInt32(cr2w, this, nameof(Unk1) ) { IsSerialized = true };
            Data = new CBytes(cr2w, this, nameof(Data) ) { IsSerialized = true };
            Collisiontypes = new CBufferVLQInt32<CName>(cr2w, this, nameof(Collisiontypes)) { IsSerialized = true };
        }

        public override void Read(BinaryReader r, uint size)
        {
            var startpos = r.BaseStream.Position;
            Unk1.Read(r, 4);
            Collisiontypes.Read(r, 0);
            var endpos = r.BaseStream.Position;
            Data.Read(r, (uint)(size - (endpos - startpos)));
        }

        public override void Write(BinaryWriter w)
        {
            Unk1.Write(w);
            Collisiontypes.Write(w);
            Data.Write(w);
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CPhysicalCollision(cr2w, parent, name);
    }
}