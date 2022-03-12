using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class TagList : CVariable
    {
        [RED] public CBufferVLQInt32<CName> tags { get; set; }

        public TagList(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
            => tags = new CBufferVLQInt32<CName>(cr2w, this, nameof(tags));

        public override void Read(BinaryReader r, uint size)
        {
            var pos = r.BaseStream.Position;
            var count = r.ReadBit6();
            for (var i = 0; i < count; i++)
            {
                var var = new CName(cr2w, tags, i.ToString());
                var.Read(r, 0);
                AddVariable(var);
            }
        }

        public override void Write(BinaryWriter w)
        {
            w.WriteBit6(tags.Count);
            for (var i = 0; i < tags.Count; i++) tags[i].Write(w);
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new TagList(cr2w, parent, name);

        public override void AddVariable(CVariable var)
        {
            if (var is CName tag) tags.Add(tag);
        }

        public override bool CanAddVariable(IEditableVariable newvar) => newvar == null || newvar is CName;

        public override bool CanRemoveVariable(IEditableVariable child) => child is CName v && tags.Contains(v);

        public override bool RemoveVariable(IEditableVariable child)
        {
            if (child is CName tag) { tags.Remove(tag); return true; }
            return false;
        }

        public override string ToString()
        {
            var list = new List<string>();
            foreach (var tag in tags) list.Add(tag.Value);
            return string.Join(", ", list);
        }
    }
}