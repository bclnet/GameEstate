using GameEstate.Red.Formats.Red.CR2W;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [REDMeta()]
    public class CGUID : CVariable
    {
        public byte[] guid;

        public CGUID(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            guid = new byte[16];
        }

        [DataMember]
        public string GuidString
        {
            get => ToString();
            set
            {
                if (Guid.TryParse(value, out var g))
                    guid = g.ToByteArray();
            }
        }

        public override void Read(BinaryReader r, uint size) => guid = r.ReadBytes(16);
        public override void Write(BinaryWriter w) => w.Write(guid);

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case byte[] o: guid = o; break;
                case CGUID cvar: guid = cvar.guid; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CGUID)base.Copy(context);
            var.guid = guid;
            return var;
        }

        public override string ToString()
        {
            if (guid != null && guid.Length > 0)
                return new Guid(guid).ToString();
            guid = new byte[16];
            return ToString();
        }
    }
}