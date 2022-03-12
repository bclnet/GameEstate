using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs.Complex
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class SEntityBufferType1 : CVariable
    {
        [Ordinal(0)] [REDBuffer] public CName ComponentName { get; set; }
        [Ordinal(1)] [REDBuffer] public CGUID Guid { get; set; }
        [Ordinal(2)] [REDBuffer] public CByteArray2 Buffer { get; set; }

        public SEntityBufferType1(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            ComponentName = new CName(cr2w, this, nameof(ComponentName)) { IsSerialized = true };
            Guid = new CGUID(cr2w, this, nameof(Guid)) { IsSerialized = true };
            Buffer = new CByteArray2(cr2w, this, nameof(Buffer)) { IsSerialized = true };
        }

        public bool CanRead(BinaryReader r)
        {
            ComponentName.Read(r, 2);
            return !string.IsNullOrEmpty(ComponentName.Value); // if empty return 0
        }

        /// <summary>
        /// Always call CanRead before calling this method! // FIXME
        /// </summary>
        /// <param name="r"></param>
        /// <param name="size"></param>
        public override void Read(BinaryReader r, uint size)
        {
            if (!string.IsNullOrEmpty(ComponentName.Value))
            {
                Guid.Read(r, 16);
                Buffer.Read(r, size);
            }
        }

        public override void Write(BinaryWriter w)
        {
            ComponentName.Write(w);
            if (!string.IsNullOrEmpty(ComponentName.Value))
            {
                Guid.Write(w);
                Buffer.Write(w);
            }
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name)
            => new SEntityBufferType1(cr2w, parent, name);

        public override CVariable Copy(CR2WCopyAction context)
        {
            var var = (SEntityBufferType1)base.Copy(context);
            var.ComponentName = (CName)ComponentName.Copy(context);
            var.Guid = (CGUID)Guid.Copy(context);
            var.Buffer = (CByteArray2)Buffer.Copy(context);
            return var;
        }

        public override string ToString() => ComponentName.Value;

        public override List<IEditableVariable> GetEditableVariables()
        {
            var editableVars = new List<IEditableVariable> { ComponentName };
            if (!string.IsNullOrEmpty(ComponentName.Value))
            {
                editableVars.Add(Guid);
                editableVars.Add(Buffer);
            }
            return editableVars;
        }
    }
}
