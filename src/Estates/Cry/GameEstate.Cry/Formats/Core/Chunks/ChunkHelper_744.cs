using System;
using System.IO;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    public class ChunkHelper_744 : ChunkHelper
    {
        public override void Read(BinaryReader r)
        {
            base.Read(r);
            HelperType = (HelperType)Enum.ToObject(typeof(HelperType), r.ReadUInt32());
            if (Version == 0x744)  // only has the Position.
            {
                Pos.X = r.ReadSingle();
                Pos.Y = r.ReadSingle();
                Pos.Z = r.ReadSingle();
            }
            else if (Version == 0x362)   // will probably never see these.
            {
                var tmpName = r.ReadChars(64);
                var stringLength = 0;
                for (int i = 0, j = tmpName.Length; i < j; i++) if (tmpName[i] == 0) { stringLength = i; break; }
                Name = new string(tmpName, 0, stringLength);
                HelperType = (HelperType)Enum.ToObject(typeof(HelperType), r.ReadUInt32());
                Pos.X = r.ReadSingle();
                Pos.Y = r.ReadSingle();
                Pos.Z = r.ReadSingle();
            }
        }
    }
}