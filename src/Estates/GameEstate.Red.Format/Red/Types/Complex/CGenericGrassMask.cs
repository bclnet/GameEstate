using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    public partial class CGenericGrassMask : CResource
    {
        [Ordinal(1000), REDBuffer(true)] public CBytes grassmask { get; set; }

        public CGenericGrassMask(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            grassmask = new CBytes(cr2w, this, nameof(grassmask))
            {
                Bytes = new byte[0],
                IsSerialized = true
            };
        }

        public override void Read(BinaryReader r, uint size)
        {
            base.Read(r, size);
            if (MaskRes == null)
                return;
            var res = MaskRes.val;
            grassmask.Bytes = r.ReadBytes((int)(res * res >> 3));
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            if (grassmask.Bytes.Length > 0)
                grassmask.Write(w);
        }

    }
}
