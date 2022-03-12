using GameEstate.Red.Formats.Red.CR2W;
using System.IO;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [REDMeta]
    public class CFloat : CVariable, IREDPrimitive
    {
        public CFloat() { }
        public CFloat(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        [DataMember] public float val { get; set; }

        public override void Read(BinaryReader r, uint size) => val = r.ReadSingle();
        public override void Write(BinaryWriter w) => w.Write(val);

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case float o: this.val = o; break;
                case CFloat cvar: this.val = cvar.val; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CFloat)base.Copy(context);
            var.val = val;
            return var;
        }

        public override string ToString() => val.ToString();
    }
}