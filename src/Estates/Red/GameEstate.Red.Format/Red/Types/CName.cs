using GameEstate.Red.Formats.Red.CR2W;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    [REDMeta()]
    public class CName : CVariable, IREDPrimitive
    {
        public CName() { }
        public CName(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        [DataMember] public string Value { get; set; }

        public override void Read(BinaryReader file, uint size)
        {
            var idx = file.ReadUInt16();
            Value = cr2w.Names[idx].Str;
        }

        /// <summary>
        /// Call after the stringtable was generated!
        /// </summary>
        /// <param name="file"></param>
        public override void Write(BinaryWriter file)
        {
            ushort val = 0;
            var nw = cr2w.Names.First(_ => _.Str == Value);
            val = (ushort)cr2w.Names.IndexOf(nw);
            file.Write(val);
        }

        public override CVariable SetValue(object val)
        {
            if (val is string v) Value = v;
            else if (val is CName cval) Value = cval.Value;
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (CName)base.Copy(context);
            var.Value = Value;
            return var;
        }

        public override string ToString() => Value;
    }
}