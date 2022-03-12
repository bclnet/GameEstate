using GameEstate.Red.Formats.Red.CR2W;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types
{
    [REDMeta]
    public class CString : CVariable, IREDPrimitive
    {
        bool _isWideChar;

        public CString() { }
        public CString(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public string val { get; set; }

        public override void Read(BinaryReader r, uint size) => val = r.ReadLengthPrefixedString();
        public override void Write(BinaryWriter w) => w.WriteLengthPrefixedString(val);

        public override CVariable SetValue(object val)
        {
            switch (val)
            {
                case string s: this.val = s; break;
                case CString cvar: this.val = cvar.val; break;
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CString)base.Copy(context);
            var.val = val;
            var._isWideChar = _isWideChar;
            return var;
        }

        public override string ToString() => val;
    }
}