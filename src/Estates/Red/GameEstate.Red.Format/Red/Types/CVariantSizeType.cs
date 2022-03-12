using GameEstate.Red.Formats.Red.CR2W;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace GameEstate.Red.Formats.Red.Types
{
    /// <summary>
    /// A generic wrapper class for a single CVariable, with the nameID left out
    /// Format: [uint size] [ushort typeID] [byte[size] data]
    /// </summary>
    [DataContract(Namespace = "")]
    [REDMeta]
    public class CVariantSizeType : CVariable, IBufferVariantAccessor
    {
        public CVariable Variant { get; set; }

        public CVariantSizeType(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public override void Read(BinaryReader r, uint size)
        {
            CVariable parsedvar = null;
            var varsize = r.ReadUInt32();
            var buffer = r.ReadBytes((int)varsize - 4);
            using (var ms = new MemoryStream(buffer))
            using (var br = new BinaryReader(ms))
            {
                var typeId = br.ReadUInt16();
                var typename = cr2w.Names[typeId].Str;
                parsedvar = CR2WTypeManager.Create(typename, nameof(Variant), cr2w, this);
                parsedvar.IsSerialized = true;
                parsedvar.Read(br, size);
            }
            Variant = parsedvar;
        }

        public override void Write(BinaryWriter w)
        {
            uint varsize = 4 + 2; //4 for the uint32 varsize, 2 for uint16 typeID
            byte[] varvalue;
            // use a temporary stream to write the variable and get the length of the variable
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                Variant.Write(bw);
                varsize += (uint)ms.Length;
                varvalue = ms.ToArray();
            }
            // write variable
            w.Write(varsize);
            w.Write(Variant.GettypeId());
            w.Write(varvalue);
        }

        public override string ToString() => Variant.ToString();

        public override CVariable Copy(CR2WCopyAction context)
        {
            var copy = (CVariantSizeType)base.Copy(context);
            if (Variant != null) copy.Variant = Variant.Copy(context);
            return copy;
        }

        public override List<IEditableVariable> GetEditableVariables() => Variant?.GetEditableVariables();

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CVariantSizeType(cr2w, parent, name);
    }
}
