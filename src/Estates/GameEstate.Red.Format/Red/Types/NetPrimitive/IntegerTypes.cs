using GameEstate.Red.Formats.Red.CR2W;
using System.IO;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [REDMeta()]
    public class CUInt64 : CVariable, IREDPrimitive
    {
        public CUInt64() { }
        public CUInt64(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        [DataMember] public ulong val { get; set; }

        public override void Read(BinaryReader r, uint size) => val = r.ReadUInt64();
        public override void Write(BinaryWriter w) => w.Write(val);

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case ulong o: this.val = o; break;
                case string s: this.val = ulong.Parse(s); break;
                case CUInt64 v: this.val = v.val; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CUInt64)base.Copy(context);
            var.val = val;
            return var;
        }

        public override string ToString() => val.ToString();
    }

    [DataContract(Namespace = "")]
    public class CUInt32 : CVariable, IREDPrimitive
    {
        public CUInt32() { }
        public CUInt32(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        [DataMember] public uint val { get; set; }

        public override void Read(BinaryReader r, uint size) => val = r.ReadUInt32();
        public override void Write(BinaryWriter w) => w.Write(val);

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case uint o: this.val = o; break;
                case string s: this.val = uint.Parse(s); break;
                case CUInt32 v: this.val = v.val; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CUInt32)base.Copy(context);
            var.val = val;
            return var;
        }

        public override string ToString() => val.ToString();
    }

    [DataContract(Namespace = "")]
    public class CUInt16 : CVariable, IREDPrimitive
    {
        public CUInt16() { }
        public CUInt16(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        [DataMember] public ushort val { get; set; }

        public override void Read(BinaryReader r, uint size) => val = r.ReadUInt16();
        public override void Write(BinaryWriter w) => w.Write(val);

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case ushort o: this.val = o; break;
                case string s: this.val = ushort.Parse(s); break;
                case CUInt16 v: this.val = v.val; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CUInt16)base.Copy(context);
            var.val = val;
            return var;
        }

        public override string ToString() => val.ToString();
    }

    [DataContract(Namespace = "")]
    public class CUInt8 : CVariable, IREDPrimitive
    {
        public CUInt8() { }
        public CUInt8(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        [DataMember] public byte val { get; set; }

        public override void Read(BinaryReader r, uint size) => val = r.ReadByte();
        public override void Write(BinaryWriter w) => w.Write(val);

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case byte o: this.val = o; break;
                case string s: this.val = byte.Parse(s); break;
                case CUInt8 v: this.val = v.val; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CUInt8)base.Copy(context);
            var.val = val;
            return var;
        }

        public override string ToString() => val.ToString();
    }

    [DataContract(Namespace = "")]
    public class CInt64 : CVariable, IREDPrimitive
    {
        public CInt64() { }
        public CInt64(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        [DataMember] public long val { get; set; }

        public override void Read(BinaryReader r, uint size) => val = r.ReadInt64();
        public override void Write(BinaryWriter w) => w.Write(val);

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case long o: this.val = o; break;
                case string s: this.val = long.Parse(s); break;
                case CInt64 v: this.val = v.val; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CInt64)base.Copy(context);
            var.val = val;
            return var;
        }

        public override string ToString() => val.ToString();
    }

    [DataContract(Namespace = "")]
    public class CInt32 : CVariable, IREDPrimitive
    {
        public CInt32() { }
        public CInt32(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        [DataMember] public int val { get; set; }

        public override void Read(BinaryReader r, uint size) => val = r.ReadInt32();
        public override void Write(BinaryWriter w) => w.Write(val);

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case int o: this.val = o; break;
                case string s: this.val = int.Parse(s); break;
                case CInt32 v: this.val = v.val; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CInt32)base.Copy(context);
            var.val = val;
            return var;
        }

        public override string ToString() => val.ToString();
    }

    [DataContract(Namespace = "")]
    public class CInt16 : CVariable, IREDPrimitive
    {
        public CInt16() { }
        public CInt16(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        [DataMember] public short val { get; set; }

        public override void Read(BinaryReader r, uint size) => val = r.ReadInt16();
        public override void Write(BinaryWriter w) => w.Write(val);

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case short o: this.val = o; break;
                case string s: this.val = short.Parse(s); break;
                case CInt16 v: this.val = v.val; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CInt16)base.Copy(context);
            var.val = val;
            return var;
        }

        public override string ToString() => val.ToString();
    }

    [DataContract(Namespace = "")]
    public class CInt8 : CVariable, IREDPrimitive
    {
        public CInt8() { }
        public CInt8(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        [DataMember] public sbyte val { get; set; }

        public override void Read(BinaryReader r, uint size) => val = r.ReadSByte();
        public override void Write(BinaryWriter w) => w.Write(val);

        public override CVariable SetValue(object newval)
        {
            switch (newval)
            {
                case sbyte o: val = o; break;
                case string s: val = sbyte.Parse(s); break;
                case CInt8 v: val = v.val; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CInt8)base.Copy(context);
            var.val = val;
            return var;
        }

        public override string ToString() => val.ToString();
    }

    [DataContract(Namespace = "")]
    public class CDynamicInt : CVariable, IREDPrimitive
    {
        public CDynamicInt() { }
        public CDynamicInt(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        [DataMember] public int val { get; set; }

        public override void Read(BinaryReader r, uint size) => val = r.ReadBit6();

        public override void Write(BinaryWriter w) => w.WriteBit6(val);

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case sbyte o: this.val = o; break;
                case string s: this.val = sbyte.Parse(s); break;
                case CDynamicInt v: this.val = v.val; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CDynamicInt)base.Copy(context);
            var.val = val;
            return var;
        }

        public override string ToString() => val.ToString();

        internal byte ToByte()
        {
            byte result;
            byte.TryParse(val.ToString(), out result);
            return result;
        }
    }

    [DataContract(Namespace = "")]
    public class CVLQInt32 : CVariable, IREDPrimitive
    {
        public CVLQInt32() { }
        public CVLQInt32(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        [DataMember] public int val { get; set; }

        public override void Read(BinaryReader r, uint size) => val = r.ReadVLQInt32();

        public override void Write(BinaryWriter w) => w.WriteVLQInt32(val);

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case sbyte o: this.val = o; break;
                case string s: this.val = sbyte.Parse(s); break;
                case CVLQInt32 v: this.val = v.val; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CVLQInt32)base.Copy(context);
            var.val = val;
            return var;
        }

        public override string ToString() => val.ToString();
    }

    [DataContract(Namespace = "")]
    public class CBool : CVariable, IREDPrimitive
    {
        public CBool() { }
        public CBool(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        byte _val;

        [DataMember]
        public bool val
        {
            get => _val != 0;
            set => _val = value ? (byte)1 : (byte)0;
        }

        public override void Read(BinaryReader r, uint size) => _val = r.ReadByte();

        public override void Write(BinaryWriter w) => w.Write(_val);

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case bool b: this.val = b; break;
                case string s: this.val = bool.Parse(s); break;
                case CBool v: this.val = v.val; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CBool)base.Copy(context);
            var._val = _val;
            return var;
        }

        public override string ToString() => val ? "True" : "False";
    }
}