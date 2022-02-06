using GameEstate.Red.Formats.Red.CR2W;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types
{
    [REDMeta()]
    public class StringAnsi : CVariable
    {
        public StringAnsi(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public string val { get; set; }

        public override void Read(BinaryReader r, uint size) => val = r.ReadLengthPrefixedStringNullTerminated();

        public override void Write(BinaryWriter w) => w.WriteLengthPrefixedStringNullTerminated(val);

        public override CVariable SetValue(object val)
        {
            if (val is string z)
                this.val = z;
            else if (val is StringAnsi cvar)
                this.val = cvar.val;
            return this;
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new StringAnsi(cr2w, parent, name);

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (StringAnsi)base.Copy(context);
            var.val = val;
            return var;
        }

        public override string ToString() => val;
    }
}