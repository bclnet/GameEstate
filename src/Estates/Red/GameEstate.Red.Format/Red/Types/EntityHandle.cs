using GameEstate.Red.Formats.Red.CR2W;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types
{
    [REDMeta]
    public class EntityHandle : CVariable
    {
        public CUInt16 id;
        public CGUID guid;
        public CBytes unk1;

        public EntityHandle(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            id = new CUInt16(cr2w, this, nameof(id));
            guid = new CGUID(cr2w, this, nameof(guid));
            unk1 = new CBytes(cr2w, this, nameof(unk1));
        }

        public override void Read(BinaryReader r, uint size)
        {
            id.Read(r, 2);
            guid.Read(r, 16);
            if (size != 0 && size - 18 > 0)
                unk1.Read(r, size - 18);
        }

        public override void Write(BinaryWriter w)
        {
            id.Write(w);
            guid.Write(w);
            unk1.Write(w);
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new EntityHandle(cr2w, parent, name);

        public override string ToString() => $"[{id}]:{guid}";

        public override List<IEditableVariable> GetEditableVariables() => new List<IEditableVariable>() { id, guid, unk1 };
    }
}