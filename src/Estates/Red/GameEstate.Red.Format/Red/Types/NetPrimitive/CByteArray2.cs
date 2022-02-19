using GameEstate.Red.Formats.Red.CR2W;
using System;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types
{
    [REDMeta()]
    public class CByteArray2 : CVariable, IByteSource
    {
        public CByteArray2(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        byte[] Bytes { get; set; }
        public byte[] GetBytes() => Bytes;

        public override void Read(BinaryReader r, uint size) => Bytes = r.ReadBytes((int)r.ReadUInt32() - 4);
        public override void Write(BinaryWriter w)
        {
            if (Bytes != null && Bytes.Length != 0)
            {
                w.Write((uint)Bytes.Length + 4);
                w.Write(Bytes);
            }
        }

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case byte[] bytes: Bytes = bytes; break;
                case CByteArray2 cvar: Bytes = cvar.Bytes; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var copy = (CByteArray2)base.Copy(context);
            if (Bytes == null) return copy;
            var newbytes = new byte[Bytes.Length];
            Bytes.CopyTo(newbytes, 0);
            copy.Bytes = newbytes;
            return copy;
        }

        public override string ToString()
        {
            if (Bytes == null)
                Bytes = Array.Empty<byte>();
            return $"{Bytes.Length} bytes";
        }
    }
}