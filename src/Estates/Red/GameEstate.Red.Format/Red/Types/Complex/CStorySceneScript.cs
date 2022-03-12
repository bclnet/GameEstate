using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using GameEstate.Red.Formats.Red.Types.Arrays;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.Complex
{
    public partial class CStorySceneScript : CStorySceneControlPart
    {
        [Ordinal(1000), REDBuffer(true)] public CCompressedBuffer<CVariant> BufferParameters { get; set; }

        public CStorySceneScript(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
            => BufferParameters = new CCompressedBuffer<CVariant>(cr2w, this, nameof(BufferParameters)) { IsSerialized = true };

        public override void Read(BinaryReader r, uint size)
        {
            base.Read(r, size);
            while (true)
            {
                var nameId = r.ReadUInt16();
                if (nameId == 0) break;
                // read cvariant
                var varname = cr2w.Names[nameId].Str;
                var cVariant = new CVariant(cr2w, BufferParameters, varname);
                cVariant.Read(r, 0);
                BufferParameters.AddVariableWithName(cVariant);
            }
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            for (var i = 0; i < BufferParameters.Count; i++)
            {
                var variable = BufferParameters[i];
                w.Write(variable.GetnameId());
                variable.Write(w);
            }
            w.Write((ushort)0);
        }
    }
}