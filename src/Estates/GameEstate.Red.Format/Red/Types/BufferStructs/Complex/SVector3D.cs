using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System;
using System.Globalization;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs.Complex
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class SVector3D : CVariable
    {
        [Ordinal(0), RED] public CFloat X { get; set; }
        [Ordinal(1), RED] public CFloat Y { get; set; }
        [Ordinal(2), RED] public CFloat Z { get; set; }

        public SVector3D(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            X = new CFloat(cr2w, this, nameof(X)) { IsSerialized = true };
            Y = new CFloat(cr2w, this, nameof(Y)) { IsSerialized = true };
            Z = new CFloat(cr2w, this, nameof(Z)) { IsSerialized = true };
        }

        public void Read(BinaryReader file, int compression)
        {
            if (compression == 0)
            {
                X.val = file.ReadSingle();
                Y.val = file.ReadSingle();
                Z.val = file.ReadSingle();
            }
            else if (compression == 1) //24 bit single
            {
                var bitsx = ReadFloat24(file);
                var bitsy = ReadFloat24(file);
                var bitsz = ReadFloat24(file);
                X.val = BitConverter.ToSingle(BitConverter.GetBytes(bitsx), 0);
                Y.val = BitConverter.ToSingle(BitConverter.GetBytes(bitsy), 0);
                Z.val = BitConverter.ToSingle(BitConverter.GetBytes(bitsz), 0);
            }
            else if (compression == 2)
            {
                var bitsx = file.ReadUInt16() << 16;
                var bitsy = file.ReadUInt16() << 16;
                var bitsz = file.ReadUInt16() << 16;
                X.val = BitConverter.ToSingle(BitConverter.GetBytes(bitsx), 0);
                Y.val = BitConverter.ToSingle(BitConverter.GetBytes(bitsy), 0);
                Z.val = BitConverter.ToSingle(BitConverter.GetBytes(bitsz), 0);
            }
        }

        uint ReadFloat24(BinaryReader file)
        {
            var pad = 0;
            var b1 = file.ReadByte();
            var b2 = file.ReadByte();
            var b3 = file.ReadByte();
            return ((uint)b3 << 24) | ((uint)b2 << 16) | ((uint)b1 << 8) | ((uint)pad);
        }

        public override void Read(BinaryReader r, uint size)
        {
            X.Read(r, size);
            Y.Read(r, size);
            Z.Read(r, size);
        }

        public override void Write(BinaryWriter w)
        {
            X.Write(w);
            Y.Write(w);
            Z.Write(w);
        }

        public override CVariable SetValue(object val)
        {
            if (val is SVector3D v)
            {
                X = v.X;
                Y = v.Y;
                Z = v.Z;
            }
            return this;
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new SVector3D(cr2w, parent, name);

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "V3[{0:0.00}, {1:0.00}, {2:0.00}]", X.val, Y.val, Z.val);
    }
}