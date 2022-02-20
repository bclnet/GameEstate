using System.IO;
using System.Text;

namespace P4KLib
{
    public static class BinaryExtensions
    {
        public static string ReadString(this BinaryReader source, int length)
        {
            var characters = source.ReadBytes(length);
            return Encoding.UTF8.GetString(characters);
        }

        public static void WriteString(this BinaryWriter source, string str, bool nullTerminated)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            source.BaseStream.Write(bytes, 0, bytes.Length);
        }
    }
}
