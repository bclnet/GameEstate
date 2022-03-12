using GameEstate.Red.Formats.Red.CR2W;
using System;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types
{
    [REDMeta]
    public class CBytes : CVariable, IByteSource
    {
        public CBytes(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public byte[] Bytes { get; set; }
        public byte[] GetBytes() => Bytes;

        public override void Read(BinaryReader r, uint size) => Bytes = r.ReadBytes((int)size);
        public override void Write(BinaryWriter w)
        {
            if (Bytes != null && Bytes.Length != 0) w.Write(Bytes);
        }

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case byte[] bytes: Bytes = bytes; break;
                case CBytes cvar: Bytes = cvar.Bytes; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var copy = (CBytes)base.Copy(context);
            if (Bytes == null) return copy;
            var newbytes = new byte[Bytes.Length];
            Bytes.CopyTo(newbytes, 0);
            copy.Bytes = newbytes;
            return copy;
        }

        public override string ToString()
        {
            if (Bytes == null) Bytes = Array.Empty<byte>();
            return $"{Bytes.Length} bytes";
        }

        public override bool CanRemoveVariable(IEditableVariable child) => false;

        public override bool CanAddVariable(IEditableVariable newvar) => true;

        public override bool RemoveVariable(IEditableVariable child) => false;

        public override void AddVariable(CVariable var)
        {
            switch (var)
            {
                case CBytes b:
                    {
                        Bytes = new byte[b.Bytes.Length];
                        Buffer.BlockCopy(b.Bytes, 0, Bytes, 0, b.Bytes.Length);
                        break;
                    }
            }
        }
    }
}