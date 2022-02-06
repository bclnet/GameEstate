using GameEstate.Red.Formats.Red.CR2W;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [REDMeta()]
    public class IdTag : CVariable
    {
        public byte _type { get; set; }
        public byte[] _guid { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string GuidString
        {
            get => new Guid(_guid).ToString();
            set
            {
                if (Guid.TryParse(value, out var g))
                    _guid = g.ToByteArray();
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public string TypeString
        {
            get => Convert.ToString(_type);
            set
            {
                if (byte.TryParse(value, out var b))
                    _type = b;
            }
        }

        public IdTag(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public override void Read(BinaryReader r, uint size)
        {
            _type = r.ReadByte();
            _guid = r.ReadBytes(16);
        }

        public override void Write(BinaryWriter w)
        {
            w.Write(_type);
            w.Write(_guid);
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (IdTag)base.Copy(context);
            var._type = _type;
            var._guid = _guid;
            return var;
        }

        public override CVariable SetValue(object val)
        {
            if (val is byte[] z) _guid = z;
            else if (val is byte z1) _type = z1;
            else if (val is IdTag cvar)
            {
                _guid = cvar._guid;
                _type = cvar._type;
            }
            return this;
        }

        public override string ToString()
        {
            if (_guid == null)
            {
                var buffer = new byte[16];
                return $"[ {_type} ] {new Guid(buffer)}";
            }
            return $"[ {_type} ] {new Guid(_guid)}";
        }
    }
}