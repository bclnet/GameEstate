using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    public partial class CTextureArray : CResource
    {
        // 24 bytes header
        [Ordinal(1000), REDBuffer] public CUInt32 texturecachekey { get; set; }
        [Ordinal(1001), REDBuffer] public CUInt16 encodedformat { get; set; }
        [Ordinal(1002), REDBuffer] public CUInt16 width { get; set; }
        [Ordinal(1003), REDBuffer] public CUInt16 height { get; set; }
        [Ordinal(1004), REDBuffer] public CUInt16 slices { get; set; }
        [Ordinal(1005), REDBuffer] public CUInt16 mipmapscount { get; set; }
        [Ordinal(1006), REDBuffer] public CUInt16 residentmip { get; set; }
        [Ordinal(1007), REDBuffer] public CUInt32 filesize { get; set; }
        [Ordinal(1008), REDBuffer] public CInt32 ffffffff { get; set; }
        [Ordinal(1009), REDBuffer(true)] public CBytes rawfile { get; set; }

        public CTextureArray(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
            => rawfile = new CBytes(cr2w, this, nameof(rawfile)) { IsSerialized = true };

        public override void Read(BinaryReader r, uint size)
        {
            base.Read(r, size);
            rawfile.Read(r, filesize.val);
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            rawfile.Write(w);
        }
    }
}