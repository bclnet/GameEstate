using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameEstate.Red.Formats.Red.CR2W
{
    public static class CR2WReaderExtensions
    {
        /// <summary>
        ///     Read null terminated string
        /// </summary>
        /// <param name="r">Reader</param>
        /// <param name="len">Fixed length string</param>
        /// <returns>string</returns>
        public static string ReadCR2WString(this BinaryReader r, int len = 0)
        {
            string str = null;
            if (len > 0) str = Encoding.GetEncoding("ISO-8859-1").GetString(r.ReadBytes(len));
            else
            {
                var b = new StringBuilder();
                while (true)
                {
                    var c = (char)r.ReadByte();
                    if (c == 0) break;
                    b.Append(c);
                }
                str = b.ToString();
            }
            return str;
        }

        public static void WriteCR2WString(this BinaryWriter w, string str)
        {
            if (str != null) w.Write(Encoding.GetEncoding("ISO-8859-1").GetBytes(str));
            w.Write((byte)0);
        }

        public static void AddUnique(this Dictionary<string, uint> dic, string str, uint val)
        {
            if (str == null) str = "";
            if (!dic.ContainsKey(str)) dic.Add(str, val);
        }

        public static uint Get(this Dictionary<string, uint> dic, string str)
        {
            if (str == null) str = "";
            return dic[str];
        }

        public static byte[] ReadRemainingData(this BinaryReader r)
            => r.ReadBytes((int)(r.BaseStream.Length - r.BaseStream.Position));
    }
}