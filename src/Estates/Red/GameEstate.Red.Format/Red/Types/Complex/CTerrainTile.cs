using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using GameEstate.Red.Formats.Red.Types.BufferStructs;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    public partial class CTerrainTile : CResource
    {
        [Ordinal(1000), REDBuffer] public CArray<STerrainTileData> Groups { get; set; }

        public CTerrainTile(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public override void Read(BinaryReader r, uint size)
        {
            base.Read(r, size);
            var maxres = r.ReadInt32();
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            var maxres = 0;
            foreach (var g in Groups)
            {
                var g3 = g.Resolution.val;
                if (g3 > maxres) maxres = g3;
            }
            w.Write(maxres);
        }
    }
}
