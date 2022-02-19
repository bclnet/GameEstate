using GameEstate.Red.Formats.Red.CR2W;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types
{
    /// <summary>
    /// A generic wrapper class for a single CVariable
    /// Format: [ushort typeID] [uint size] [byte[size] data]
    /// </summary>
    [REDMeta()]
    public class CVariant : CVariable, IVariantAccessor
    {
        public CVariable Variant { get; set; }

        public CVariant(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public override void Read(BinaryReader r, uint size)
        {
            var typepos = r.BaseStream.Position;
            var typeId = r.ReadUInt16();
            var typename = cr2w.Names[typeId].Str;
            var varsize = r.ReadUInt32() - 4;
            if (varsize > 0)
            {
                Variant = CR2WTypeManager.Create(typename, nameof(Variant), cr2w, this);
                Variant.Read(r, varsize);
                Variant.IsSerialized = true;
            }
            else { } // do nothing I guess?
        }

        public override void Write(BinaryWriter w)
        {
            if (Variant == null)
            {
                w.Write((ushort)0);
                w.Write((uint)4);
            }
            else
            {
                w.Write(Variant.GettypeId());
                var buffer = System.Array.Empty<byte>();
                using (var ms = new MemoryStream())
                using (var bw = new BinaryWriter(ms))
                {
                    Variant.Write(bw);
                    buffer = ms.ToArray();
                }
                w.Write(buffer.Length + 4);
                w.Write(buffer);
            }
        }

        public override CVariable SetValue(object val)
        {
            //if (val is CVariable)
            //{
            //    Variant = (CVariable)val;
            //}
            /*else*/
            if (val is CVariant cvar)
            {
                var context = new CR2WCopyAction()
                {
                    DestinationFile = cr2w,
                    Parent = ParentVar as CVariable
                };
                Variant = cvar.Variant.Copy(context);
            }
            return this;
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var copy = (CVariant) base.Copy(context);
            if (Variant != null)
                copy.Variant = Variant.Copy(context);
            return copy;
        }

        public override List<IEditableVariable> GetEditableVariables() => Variant?.GetEditableVariables();

        public override string ToString() => Variant != null ? Variant.ToString() : "";
    }
}