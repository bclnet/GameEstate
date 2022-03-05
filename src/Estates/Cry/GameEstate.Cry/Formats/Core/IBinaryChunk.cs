using System.IO;

namespace GameEstate.Cry.Formats.Core
{
    public interface IBinaryChunk
    {
        void Read(BinaryReader r);
        void Write(BinaryWriter w);
    }
}
