using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    public partial class CLayerInfo : ISerializable
    {
        [Ordinal(1000), REDBuffer(true)] public CHandle<CLayerGroup> ParentGroup { get; set; }

        public CLayerInfo(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
            => ParentGroup = new CHandle<CLayerGroup>(cr2w, this, nameof(ParentGroup)) { IsSerialized = true };

        public override void Read(BinaryReader r, uint size)
        {
            var startpos = r.BaseStream.Position;
            base.Read(r, size);
            var endpos = r.BaseStream.Position;
            var bytesread = endpos - startpos;
            if (bytesread != size)
            {
                ParentGroup.ChunkHandle = true;
                ParentGroup.Read(r, 4);
            }
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            // HACK check if it has been set in Read()
            if (ParentGroup != null && ParentGroup.ChunkHandle == true) ParentGroup.Write(w);
        }
    }
}
