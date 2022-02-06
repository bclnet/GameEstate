using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs.Complex
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class SEntityBufferType2 : CVariable
    {
        [Ordinal(0), REDBuffer] public CName componentName { get; set; }
        [Ordinal(1), REDBuffer] public CUInt32 sizeofdata { get; set; }
        [Ordinal(2), REDBuffer] public CBufferUInt32<CVariantSizeTypeName> variables { get; set; }

        public SEntityBufferType2(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            componentName = new CName(cr2w, this, nameof(componentName)) { IsSerialized = true };
            sizeofdata = new CUInt32(cr2w, this, nameof(sizeofdata)) { IsSerialized = true };
            variables = new CBufferUInt32<CVariantSizeTypeName>(cr2w, this, nameof(variables)) { IsSerialized = true };
        }

        public override void Read(BinaryReader r, uint size)
        {
            sizeofdata.Read(r, 4);
            componentName.Read(r, 2);
            variables.Read(r, size);
        }

        public override void Write(BinaryWriter w)
        {
            sizeofdata.val = 4; // 4 for the uint32 varsize
            byte[] buffer;
            // use a temporary stream to write the variables and get the overall length of the component
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                componentName.Write(bw);
                variables.Write(bw);
                sizeofdata.val += (UInt32)ms.Length;
                buffer = ms.ToArray();
            }
            sizeofdata.Write(w);
            w.Write(buffer);
        }

        public override string ToString() => componentName.Value;

        public override List<IEditableVariable> GetEditableVariables() => base.GetEditableVariables();
    }
}
