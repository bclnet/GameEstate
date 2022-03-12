using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    public partial class CFont : CResource
    {
        [Ordinal(1000), REDBuffer(true)] public CArray<CUInt16> Unicodemapping { get; set; }
        [Ordinal(1001), REDBuffer(true)] public CInt32 Linedist { get; set; }
        [Ordinal(1002), REDBuffer(true)] public CInt32 Maxglyphheight { get; set; }
        [Ordinal(1003), REDBuffer(true)] public CBool Kerning { get; set; }
        [Ordinal(1004), REDBuffer(true)] public CArray<CArray<CFloat>> Glyphs { get; set; }

        public CFont(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            Unicodemapping = new CArray<CUInt16>(cr2w, this, nameof(Unicodemapping)) { IsSerialized = true, Elementtype = "Uint16" };
            Linedist = new CInt32(cr2w, this, nameof(Linedist)) { IsSerialized = true };
            Maxglyphheight = new CInt32(cr2w, this, nameof(Maxglyphheight)) { IsSerialized = true };
            Kerning = new CBool(cr2w, this, nameof(Kerning)) { IsSerialized = true };
            Glyphs = new CArray<CArray<CFloat>>(cr2w, this, nameof(Glyphs)) { IsSerialized = true, Elementtype = "array:2,0,Float" };
        }

        public override void Read(BinaryReader r, uint size)
        {
            base.Read(r, size);

            var cnt = r.ReadVLQInt32();
            for (int i = 0; i < cnt; i++)
            {
                //This is actually a byte-byte pair but no idea why or how anyone would edit this
                var mapping = new CUInt16(cr2w, Unicodemapping, "");
                mapping.Read(r, size);
                Unicodemapping.AddVariable(mapping);
            }
            Linedist.Read(r, size);
            Maxglyphheight.Read(r, size);
            Kerning.Read(r, size);

            var num = r.ReadVLQInt32();

            for (var i = 0; i < num; i++)
            {
                var glyph = new CArray<CFloat>(cr2w, Glyphs, $"Glyph - {i}") { Elementtype = "Float" };
                // UVs
                var uv00 = new CFloat(cr2w, glyph, "UV[0][0]");
                uv00.Read(r, size);
                glyph.AddVariable(uv00);
                var uv01 = new CFloat(cr2w, glyph, "UV[0][1]");
                uv01.Read(r, size);
                glyph.AddVariable(uv01);
                var uv10 = new CFloat(cr2w, glyph, "UV[1][0]");
                uv10.Read(r, size);
                glyph.AddVariable(uv10);
                var uv11 = new CFloat(cr2w, glyph, "UV[1][1]");
                uv11.Read(r, size);
                glyph.AddVariable(uv11);

                var textureindex = new CInt32(cr2w, glyph, "Texture index");
                textureindex.Read(r, size);
                glyph.AddVariable(textureindex);
                var width = new CInt32(cr2w, glyph, "Width");
                width.Read(r, size);
                glyph.AddVariable(width);
                var height = new CInt32(cr2w, glyph, "Height");
                height.Read(r, size);
                glyph.AddVariable(height);
                var advance_x = new CInt32(cr2w, glyph, "Advance X");
                advance_x.Read(r, size);
                glyph.AddVariable(advance_x);
                var bearing_x = new CInt32(cr2w, glyph, "Bearing X");
                bearing_x.Read(r, size);
                glyph.AddVariable(bearing_x);
                var bearing_y = new CInt32(cr2w, glyph, "Bearing Y");
                bearing_y.Read(r, size);
                glyph.AddVariable(bearing_y);

                Glyphs.AddVariable(glyph);
            }
        }

        public override void Write(BinaryWriter file)
        {
            base.Write(file);
            file.WriteVLQInt32(Unicodemapping.Count);
            foreach (var mapping in Unicodemapping) mapping.Write(file);

            Linedist.Write(file);
            Maxglyphheight.Write(file);
            Kerning.Write(file);

            file.WriteVLQInt32(Glyphs.Count);

            for (var i = 0; i < Glyphs.Count; i++)
                foreach (var subelement in (Glyphs[i]).Elements) subelement.Write(file);
        }
    }
}
