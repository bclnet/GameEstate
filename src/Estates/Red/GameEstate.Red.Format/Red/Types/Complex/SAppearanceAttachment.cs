using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    public partial class SAppearanceAttachment : CVariable
    {
        [Ordinal(1000), REDBuffer(true)] public CBufferVLQInt32<IReferencable> Data { get; set; }

        public SAppearanceAttachment(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            Data = new CBufferVLQInt32<IReferencable>(cr2w, this, nameof(Data)) { IsSerialized = true };
        }

        public override void Read(BinaryReader r, uint size)
        {
            base.Read(r, size);
            var startpos = r.BaseStream.Position;
            var bytecount = r.ReadUInt32();
            // read classes
            var count = r.ReadBit6();
            for (var i = 0; i < count; i++)
            {
                var className = new CName(cr2w, null, "");
                className.Read(r, 2);
                var parsedvar = CR2WTypeManager.Create(className.Value, "", cr2w, Data);
                parsedvar.Read(r, size);
                Data.AddVariable(parsedvar);
            }

            //check
            var endpos = r.BaseStream.Position;
            var bytesread = endpos - startpos;
            if (bytesread != bytecount)
                throw new InvalidParsingException($"Error in parsing SAppearanceAttachment: Data Variable. Bytes read: {bytesread} out of {bytecount}.");
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            byte[] buffer;
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.WriteBit6(Data.elements.Count);
                foreach (var cvar in Data.elements)
                {
                    var className = new CName(cr2w, null, "")
                    {
                        Value = cvar.REDType
                    };
                    className.Write(bw);
                    cvar.Write(bw);
                }
                buffer = ms.ToArray();
            }
            w.Write(buffer.Length + 4);
            w.Write(buffer);
        }
    }
}