using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.IO;
using System.Linq;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    public partial class CBitmapTexture : ITexture, IByteSource
    {
        [Ordinal(1000), REDBuffer] public CUInt32 unk { get; set; }
        [Ordinal(1001), REDBuffer(true)] public CUInt32 MipsCount { get; set; }
        [Ordinal(1002), REDBuffer(true)] public CCompressedBuffer<SMipData> Mipdata { get; set; }
        [Ordinal(1003), REDBuffer(true)] public CUInt16 unk1 { get; set; }
        [Ordinal(1003), REDBuffer(true)] public CUInt16 unk2 { get; set; }
        // Uncooked Textures
        // Cooked Textures
        [Ordinal(1005), REDBuffer(true)] public CUInt32 ResidentmipSize { get; set; }
        [Ordinal(1006), REDBuffer(true)] public CBytes Residentmip { get; set; }

        public CBitmapTexture(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            MipsCount = new CUInt32(cr2w, this, nameof(MipsCount)) { IsSerialized = true };
            Mipdata = new CCompressedBuffer<SMipData>(cr2w, this, nameof(Mipdata)) { IsSerialized = true };
            unk1 = new CUInt16(cr2w, this, nameof(unk1)) { IsSerialized = true };
            unk2 = new CUInt16(cr2w, this, nameof(unk2)) { IsSerialized = true };
            ResidentmipSize = new CUInt32(cr2w, this, nameof(ResidentmipSize)) { IsSerialized = true };
            Residentmip = new CBytes(cr2w, this, nameof(Residentmip)) { IsSerialized = true };
        }

        public byte[] GetBytes()
        {
            var isUncooked = REDFlags == 0;
            byte[] bytesource;
            if (isUncooked)
            {
                if (Mipdata.Count <= 0) return null;
                bytesource = Mipdata.First().Mip.Bytes;
                for (var index = 1; index < Mipdata.Count; index++)
                {
                    var byteArray = Mipdata[index].Mip;
                    bytesource = bytesource.Concat(byteArray.Bytes).ToArray();
                }
            }
            else bytesource = Residentmip.Bytes;
            return bytesource;
        }

        public override void Read(BinaryReader r, uint size)
        {
            base.Read(r, size);
            //TODO: readd for tw3
            //MipsCount.Read(r, 4);
            //Mipdata.Read(r, size, (int)MipsCount.val);
            //ResidentmipSize.Read(r, 4);
            //unk1.Read(r, 2);
            //unk2.Read(r, 2);
            //Residentmip.Read(r, ResidentmipSize.val);
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            MipsCount.val = (uint)Mipdata.Count;
            MipsCount.Write(w);
            Mipdata.Write(w);
            ResidentmipSize.Write(w);
            unk1.Write(w);
            unk2.Write(w);
            Residentmip.Write(w);
        }
    }
}
