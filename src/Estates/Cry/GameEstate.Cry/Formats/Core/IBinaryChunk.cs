using System.IO;

namespace GameEstate.Cry.Formats.Core
{
    public interface IBinaryChunk
    {
        void Read(BinaryReader reader);
        void Write(BinaryWriter writer);
    }
}
