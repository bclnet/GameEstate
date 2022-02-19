using GameEstate.Red.Formats.Red.CR2W;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    /// <summary>
    /// A generic wrapper class for a single CVariable
    /// Format: [uint size] [ushort nameID] [ushort typeID] [byte[size] data]
    /// </summary>
    [DataContract(Namespace = ""), REDMeta()]
    public class CVariantSizeNameType : CVariable, IBufferVariantAccessor
    {
        public CVariable Variant { get; set; }

        public CVariantSizeNameType(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public override void Read(BinaryReader r, uint size)
        {
            CVariable parsedvar = null;
            var varsize = r.ReadUInt32();
            var buffer = r.ReadBytes((int)varsize - 4);
            using (var ms = new MemoryStream(buffer))
            using (var br = new BinaryReader(ms))
            {
                var nameId = br.ReadUInt16();
                var typeId = br.ReadUInt16();
                if (nameId == 0)
                    return;

                var typename = cr2w.Names[typeId].Str;
                var varname = cr2w.Names[nameId].Str;
                parsedvar = CR2WTypeManager.Create(typename, varname, cr2w, this);
                parsedvar.IsSerialized = true;
                parsedvar.Read(br, size);
                SetREDName(varname);
            }
            Variant = parsedvar;
        }

        public override void Write(BinaryWriter w)
        {
            var varsize = 4 + 4; //4 for the uint32 varsize, 4 for 2 x uint16 typeID and nameID
            byte[] varvalue;

            // use a temporary stream to write the variable and get the length of the variable
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                Variant.Write(bw);
                varsize += (int)ms.Length;
                varvalue = ms.ToArray();
            }

            // write variable
            w.Write(varsize);
            w.Write(Variant.GetnameId());
            w.Write(Variant.GettypeId());
            w.Write(varvalue);
        }

        public override string ToString() => Variant == null ? "NULL" : Variant.ToString();

        public override CVariable Copy(CR2WCopyAction context)
        {
            var copy = (CVariantSizeNameType)base.Copy(context);
            if (Variant != null)
                copy.Variant = Variant.Copy(context);
            return copy;
        }

        public override List<IEditableVariable> GetEditableVariables() => Variant?.GetEditableVariables();
        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CVariantSizeNameType(cr2w, parent, name);
    }
}
