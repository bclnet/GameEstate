using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.IO;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    [DataContract(Namespace = ""), REDMeta(EREDMetaInfo.REDStruct)]
    public class EngineTransform : CVariable
    {
        public byte flags;
        public string type;

        [Ordinal(1), RED] public CFloat Pitch { get; set; }
        [Ordinal(2), RED] public CFloat Yaw { get; set; }
        [Ordinal(3), RED] public CFloat Roll { get; set; }
        [Ordinal(4), RED] public CFloat Scale_x { get; set; }
        [Ordinal(5), RED] public CFloat Scale_y { get; set; }
        [Ordinal(6), RED] public CFloat Scale_z { get; set; }
        [Ordinal(7), RED] public CFloat X { get; set; }
        [Ordinal(8), RED] public CFloat Y { get; set; }
        [Ordinal(9), RED] public CFloat Z { get; set; }

        public EngineTransform(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            X = new CFloat(cr2w, this, nameof(X));
            Y = new CFloat(cr2w, this, nameof(Y));
            Z = new CFloat(cr2w, this, nameof(Z));
            Pitch = new CFloat(cr2w, this, nameof(Pitch));
            Yaw = new CFloat(cr2w, this, nameof(Yaw));
            Roll = new CFloat(cr2w, this, nameof(Roll));
            Scale_x = new CFloat(cr2w, this, nameof(Scale_x));
            Scale_y = new CFloat(cr2w, this, nameof(Scale_y));
            Scale_z = new CFloat(cr2w, this, nameof(Scale_z));
        }

        public override void Read(BinaryReader r, uint size)
        {
            flags = r.ReadByte();
            if ((flags & 1) == 1)
            {
                X.Read(r, 4);
                Y.Read(r, 4);
                Z.Read(r, 4);
            }
            if ((flags & 2) == 2)
            {
                Pitch.Read(r, 4);
                Yaw.Read(r, 4);
                Roll.Read(r, 4);
            }
            if ((flags & 4) == 4)
            {
                Scale_x.Read(r, 4);
                Scale_y.Read(r, 4);
                Scale_z.Read(r, 4);
            }
        }

        public override void Write(BinaryWriter w)
        {
            flags = 0;
            if (X.val != 0 || Y.val != 0 || Z.val != 0)
                flags |= 1;
            if (Pitch.val != 0 || Yaw.val != 0 || Roll.val != 0)
                flags |= 2;
            if (Scale_x.val != 0 || Scale_y.val != 0 || Scale_z.val != 0)
                flags |= 4;
            w.Write(flags);
            if ((flags & 1) == 1)
            {
                X.Write(w);
                Y.Write(w);
                Z.Write(w);
            }
            if ((flags & 2) == 2)
            {
                Pitch.Write(w);
                Yaw.Write(w);
                Roll.Write(w);
            }
            if ((flags & 4) == 4)
            {
                Scale_x.Write(w);
                Scale_y.Write(w);
                Scale_z.Write(w);
            }
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new EngineTransform(cr2w, parent, name);
    }
}