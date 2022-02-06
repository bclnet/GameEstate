using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    public partial class CSwfTexture : CBitmapTexture
    {
        //[Ordinal(1000), REDBuffer(true)] public CBytes swfTexture { get; set; }

        public CSwfTexture(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            //swfTexture = new CBytes(cr2w, this, nameof(swfTexture));
        }

        public override void Read(BinaryReader r, uint size)
        {
            // var pos = r.BaseStream.Position;
            base.Read(r, size);
            //var textureSize = Convert.ToInt32(size - (file.BaseStream.Position - pos));
            //swfTexture.Read(r, (uint)textureSize);
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            //swfTexture.Write(w);
        }
    }
}