using FastMember;
using GameEstate.Red.Formats.Red.CR2W;
using System.Diagnostics;
using System.IO;

namespace GameEstate.Red.Formats.Red.Types.BufferStructs.Complex
{
    [REDMeta(EREDMetaInfo.REDStruct)]
    public class CurveInfo : CVariable
    {
        [Ordinal(1), RED] public CName someName { get; set; }
        [Ordinal(2), RED] public CUInt8 someByte { get; set; }
        [Ordinal(3), RED] public CCompressedBuffer<CurvePiece> pieces { get; set; } // MAX LIMIT 4

        public CurveInfo(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name)
        {
            someName = new CName(cr2w, this, nameof(someName)) { IsSerialized = true };
            someByte = new CUInt8(cr2w, this, nameof(someByte)) { IsSerialized = true };
            pieces = new CCompressedBuffer<CurvePiece>(cr2w, this, nameof(pieces)) { IsSerialized = true };
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var copy = base.Copy(context) as CurveInfo;
            copy.someName = someName.Copy(context) as CName;
            copy.someByte = someByte.Copy(context) as CUInt8;
            foreach (var piece in pieces)
                copy.pieces.Add(piece.Copy(context) as CurvePiece);
            return copy;
        }

        public static CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CurveInfo(cr2w, parent, name);

        public override void Read(BinaryReader file, uint size)
        {
            someName.Read(file, size);
            var count = file.ReadByte();
            if (count > 4)
            {
                Debug.Print($"Curve piece count out of bounds: {count}, using as 4");
                count = 4;
            }
            someByte.Read(file, size);
            pieces.Read(file, size, count);
        }

        public override void Write(BinaryWriter file)
        {
            var pieceCount = (byte)pieces.Count;
            if (pieceCount > 4)
            {
                Debug.Print($"Cannot write more than 4 pieces ({pieceCount}) for curve info, limiting");
                pieceCount = 4;
            }
            someName.Write(file);
            file.Write(pieceCount);
            someByte.Write(file);
            pieces.Write(file);
        }
    }

}