using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace System.IO
{
    public static class Polyfill
    {
        public enum ASCIIFormat { PossiblyNullTerminated, ZeroPadded, ZeroTerminated }

        #region Stream

        //public static byte[] ReadAllBytes(this Stream stream)
        //{
        //    using var b = new MemoryStream();
        //    stream.Position = 0;
        //    stream.CopyTo(b);
        //    return b.ToArray();
        //}

        public static byte[] ReadBytes(this Stream stream, int count) { var data = new byte[count]; stream.Read(data, 0, count); return data; }
        public static void WriteBytes(this Stream stream, byte[] data) => stream.Write(data, 0, data.Length);
        public static void WriteBytes(this Stream stream, BinaryReader r, int count) { var data = r.ReadBytes(count); stream.Write(data, 0, data.Length); }

        #endregion

        #region BinaryWriter

        public static long Position(this BinaryWriter source) => source.BaseStream.Position;
        public static void WriteBytes(this BinaryWriter source, byte[] data) => source.Write(data, 0, data.Length);
        public static void WriteT<T>(this BinaryWriter source, T value, int length) => source.WriteBytes(UnsafeX.MarshalF(value, length));

        #endregion

        #region BinaryReader

        /// <summary>
        /// A Compressed UInt32 can be 1, 2, or 4 bytes.<para />
        /// If the first MSB (0x80) is 0, it is one byte.<para />
        /// If the first MSB (0x80) is set and the second MSB (0x40) is 0, it's 2 bytes.<para />
        /// If both (0x80) and (0x40) are set, it's 4 bytes.
        /// </summary>
        public static uint ReadCompressedUInt32(this BinaryReader source)
        {
            var b0 = source.ReadByte();
            if ((b0 & 0x80) == 0) return b0;
            var b1 = source.ReadByte();
            if ((b0 & 0x40) == 0) return (uint)(((b0 & 0x7F) << 8) | b1);
            var s = source.ReadUInt16();
            return (uint)(((((b0 & 0x3F) << 8) | b1) << 16) | s);
        }

        /// <summary>
        /// Aligns the stream to the next DWORD boundary.
        /// </summary>
        public static void AlignBoundary(this BinaryReader source)
        {
            var alignDelta = source.BaseStream.Position % 4;
            if (alignDelta != 0) source.BaseStream.Position += (int)(4 - alignDelta);
        }

        public static long Position(this BinaryReader source) => source.BaseStream.Position;
        public static void Position(this BinaryReader source, long position) => source.BaseStream.Position = position;
        public static long Position(this BinaryReader source, long position, int align) { if (position % 4 != 0) position += 4 - (position % 4); source.BaseStream.Position = position; return position; }
        public static void Seek(this BinaryReader source, long offset, SeekOrigin origin) => source.BaseStream.Seek(offset, origin);
        public static void Skip(this BinaryReader source, long count) => source.BaseStream.Position += count; //source.BaseStream.Seek(count, SeekOrigin.Current);

        public static void Peek(this BinaryReader source, Action<BinaryReader> action, int offset = 0)
        {
            var position = source.BaseStream.Position;
            if (offset != 0) source.BaseStream.Position += offset;
            action(source);
            source.BaseStream.Position = position;
        }
        public static T Peek<T>(this BinaryReader source, Func<BinaryReader, T> action, int offset = 0)
        {
            var position = source.BaseStream.Position;
            if (offset != 0) source.BaseStream.Position += offset;
            var value = action(source);
            source.BaseStream.Position = position;
            return value;
        }

        public static void CopyTo(this BinaryReader source, Stream destination, bool resetPosition = true)
        {
            source.BaseStream.CopyTo(destination);
            if (resetPosition) destination.Position = 0;
        }

        public static byte[] ReadAbsoluteBytes(this BinaryReader source, long position, int count)
        {
            var last = source.BaseStream.Position;
            source.BaseStream.Position = position;
            var r = source.ReadBytes(count);
            source.BaseStream.Position = last;
            return r;
        }

        public static byte[] ReadToEnd(this BinaryReader source)
        {
            var length = (int)(source.BaseStream.Length - source.BaseStream.Position);
            Debug.Assert(length <= int.MaxValue);
            return source.ReadBytes(length);
        }
        public static void ReadToEnd(this BinaryReader source, byte[] buffer, int startIndex = 0)
        {
            var length = (int)source.BaseStream.Length - source.BaseStream.Position;
            Debug.Assert(startIndex >= 0 && length <= int.MaxValue && startIndex + length <= buffer.Length);
            source.Read(buffer, startIndex, (int)length);
        }

        /// <summary>
        /// First reads a UInt16. If the MSB is set, it will be masked with 0x3FFF, shifted left 2 bytes, and then OR'd with the next UInt16. The sum is then added to knownType.
        /// </summary>
        public static uint ReadAsDataIDOfKnownType(this BinaryReader source, uint knownType)
        {
            var value = source.ReadUInt16();
            if ((value & 0x8000) != 0)
            {
                var lower = source.ReadUInt16();
                var higher = (value & 0x3FFF) << 16;
                return (uint)(knownType + (higher | lower));
            }
            return knownType + value;
        }

        public static Guid ReadGuid(this BinaryReader source) => new Guid(source.ReadBytes(16));

        public static string ReadString(this BinaryReader source, int length) => new string(source.ReadChars(length));
        public static string ReadZString(this BinaryReader source, char endChar = '\0', StringBuilder builder = null)
        {
            var b = builder ?? new StringBuilder();
            char c;
            while ((c = source.ReadChar()) != endChar) b.Append(c);
            var value = b.ToString();
            if (builder != null) builder.Length = 0;
            return value;
        }

        public static string ReadUnicodeString(this BinaryReader source)
        {
            var stringLength = source.ReadCompressedUInt32();
            var thestring = "";
            for (var i = 0; i < stringLength; i++)
            {
                var myChar = source.ReadUInt16();
                thestring += Convert.ToChar(myChar);
            }
            return thestring;
        }

        public static string ReadObfuscatedString(this BinaryReader source)
        {
            var stringlength = source.ReadUInt16();
            var thestring = source.ReadBytes(stringlength);
            // flip the bytes in the string to undo the obfuscation: i.e. 0xAB => 0xBA
            for (var i = 0; i < stringlength; i++) thestring[i] = (byte)((thestring[i] >> 4) | (thestring[i] << 4));
            return Encoding.GetEncoding(1252).GetString(thestring);
        }

        public static byte[] ReadL32Bytes(this BinaryReader source) => source.ReadBytes((int)source.ReadUInt32());

        public static string ReadL8ANSI(this BinaryReader source, Encoding encoding = null) => (encoding ?? Encoding.ASCII).GetString(source.ReadBytes(source.ReadByte()));
        public static string ReadL16ANSI(this BinaryReader source, Encoding encoding = null) => (encoding ?? Encoding.ASCII).GetString(source.ReadBytes(source.ReadUInt16()));
        public static string ReadL16ANSI(this BinaryReader source, bool nullTerminated, Encoding encoding = null) { var bytes = source.ReadBytes(source.ReadUInt16()); var newLength = bytes.Length - 1; return (encoding ?? Encoding.ASCII).GetString(bytes, 0, nullTerminated && bytes[newLength] == 0 ? newLength : bytes.Length); }
        public static string ReadL32ANSI(this BinaryReader source, Encoding encoding = null) => (encoding ?? Encoding.ASCII).GetString(source.ReadBytes((int)source.ReadUInt32()));
        public static string ReadL32ANSI(this BinaryReader source, bool nullTerminated, Encoding encoding = null) { var bytes = source.ReadBytes((int)source.ReadUInt32()); var newLength = bytes.Length - 1; return (encoding ?? Encoding.ASCII).GetString(bytes, 0, nullTerminated && bytes[newLength] == 0 ? newLength : bytes.Length); }
        public static string ReadC32ANSI(this BinaryReader source, Encoding encoding = null) => (encoding ?? Encoding.ASCII).GetString(source.ReadBytes((int)source.ReadCompressedUInt32()));
        public static string ReadC32ANSI(this BinaryReader source, bool nullTerminated, Encoding encoding = null) { var bytes = source.ReadBytes((int)source.ReadCompressedUInt32()); var newLength = bytes.Length - 1; return (encoding ?? Encoding.ASCII).GetString(bytes, 0, nullTerminated && bytes[newLength] == 0 ? newLength : bytes.Length); }
        public static string ReadANSI(this BinaryReader source, int length, Encoding encoding = null) => (encoding ?? Encoding.ASCII).GetString(source.ReadBytes(length));
        public static string ReadANSI(this BinaryReader source, int length, ASCIIFormat format, Encoding encoding = null)
        {
            var buf = source.ReadBytes(length);
            int i;
            switch (format)
            {
                case ASCIIFormat.PossiblyNullTerminated:
                    i = buf[^1] != 0 ? buf.Length : buf.Length - 1;
                    return (encoding ?? Encoding.ASCII).GetString(buf, 0, i);
                case ASCIIFormat.ZeroPadded:
                    for (i = buf.Length - 1; i >= 0 && buf[i] == 0; i--) { }
                    return (encoding ?? Encoding.ASCII).GetString(buf, 0, i + 1);
                case ASCIIFormat.ZeroTerminated:
                    for (i = 0; i < buf.Length && buf[i] != 0; i++) { }
                    return (encoding ?? Encoding.ASCII).GetString(buf, 0, i);
                default: throw new ArgumentOutOfRangeException(nameof(format), format.ToString());
            }
        }

        public static string ReadZEncoding(this BinaryReader source, Encoding encoding)
        {
            var characterSize = encoding.GetByteCount("e");
            using var s = new MemoryStream();
            while (true)
            {
                var data = new byte[characterSize];
                source.Read(data, 0, characterSize);
                if (encoding.GetString(data, 0, characterSize) == "\0") break;
                s.Write(data, 0, data.Length);
            }
            return encoding.GetString(s.ToArray());
        }

        public static string ReadO32Encoding(this BinaryReader source, Encoding encoding)
        {
            var currentOffset = source.BaseStream.Position;
            var offset = source.ReadUInt32();
            if (offset == 0) return string.Empty;
            source.BaseStream.Position = currentOffset + offset;
            var str = ReadZEncoding(source, encoding);
            source.BaseStream.Position = currentOffset + 4;
            return str;
        }

        public static string ReadO32UTF8(this BinaryReader source)
        {
            var currentOffset = source.BaseStream.Position;
            var offset = source.ReadUInt32();
            if (offset == 0) return string.Empty;
            source.BaseStream.Position = currentOffset + offset;
            var str = ReadZUTF8(source);
            source.BaseStream.Position = currentOffset + 4;
            return str;
        }

        public static string ReadZUTF8(this BinaryReader source, int length = int.MaxValue, MemoryStream buf = null)
        {
            if (buf == null) buf = new MemoryStream();
            buf.SetLength(0);
            byte c;
            while (length-- > 0 && (c = source.ReadByte()) != 0) buf.WriteByte(c);
            return Encoding.UTF8.GetString(buf.ToArray());
        }
        public static string ReadZASCII(this BinaryReader source, int length = int.MaxValue, MemoryStream buf = null)
        {
            if (buf == null) buf = new MemoryStream();
            buf.SetLength(0);
            byte c;
            while (length-- > 0 && (c = source.ReadByte()) != 0) buf.WriteByte(c);
            return Encoding.ASCII.GetString(buf.ToArray());
        }
        public static string[] ReadZASCIIArray(this BinaryReader source, int length = int.MaxValue, MemoryStream buf = null)
        {
            if (buf == null) buf = new MemoryStream();
            var list = new List<string>();
            byte c;
            while (length > 0)
            {
                buf.SetLength(0);
                while (length-- > 0 && (c = source.ReadByte()) != 0) buf.WriteByte(c);
                list.Add(Encoding.ASCII.GetString(buf.ToArray()));
            }
            return list.ToArray();
        }

        public static T ReadT<T>(this BinaryReader source, int sizeOf) => UnsafeX.MarshalT<T>(source.ReadBytes(sizeOf));

        public static T[] ReadL16Array<T>(this BinaryReader source, int sizeOf) => ReadTArray<T>(source, sizeOf, source.ReadUInt16());
        public static T[] ReadL32Array<T>(this BinaryReader source, int sizeOf) => ReadTArray<T>(source, sizeOf, (int)source.ReadUInt32());
        public static T[] ReadC32Array<T>(this BinaryReader source, int sizeOf) => ReadTArray<T>(source, sizeOf, (int)source.ReadCompressedUInt32());
        public static T[] ReadTArray<T>(this BinaryReader source, int sizeOf, int count) => UnsafeX.MarshalTArray<T>(source.ReadBytes(count * sizeOf), 0, count);

        public static T[] ReadL16Array<T>(this BinaryReader source, Func<BinaryReader, T> factory) => ReadTArray(source, factory, source.ReadUInt16());
        public static T[] ReadL32Array<T>(this BinaryReader source, Func<BinaryReader, T> factory) => ReadTArray(source, factory, (int)source.ReadUInt32());
        public static T[] ReadC32Array<T>(this BinaryReader source, Func<BinaryReader, T> factory) => ReadTArray(source, factory, (int)source.ReadCompressedUInt32());
        public static T[] ReadTArray<T>(this BinaryReader source, Func<BinaryReader, T> factory, int count) { var list = new T[count]; for (var i = 0; i < list.Length; i++) list[i] = factory(source); return list; }

        public static Dictionary<TKey, TValue> ReadL16Many<TKey, TValue>(this BinaryReader source, int keySizeOf, Func<BinaryReader, TValue> valueFactory, int offset = 0) => ReadTMany<TKey, TValue>(source, keySizeOf, valueFactory, source.ReadUInt16(), offset);
        public static Dictionary<TKey, TValue> ReadL32Many<TKey, TValue>(this BinaryReader source, int keySizeOf, Func<BinaryReader, TValue> valueFactory, int offset = 0) => ReadTMany<TKey, TValue>(source, keySizeOf, valueFactory, (int)source.ReadUInt32(), offset);
        public static Dictionary<TKey, TValue> ReadC32Many<TKey, TValue>(this BinaryReader source, int keySizeOf, Func<BinaryReader, TValue> valueFactory, int offset = 0) => ReadTMany<TKey, TValue>(source, keySizeOf, valueFactory, (int)source.ReadCompressedUInt32(), offset);
        public static Dictionary<TKey, TValue> ReadTMany<TKey, TValue>(this BinaryReader source, int keySizeOf, Func<BinaryReader, TValue> valueFactory, int count, int offset = 0)
        {
            if (offset != 0) source.Skip(offset);
            var set = new Dictionary<TKey, TValue>();
            for (var i = 0; i < count; i++) set.Add(source.ReadT<TKey>(keySizeOf), valueFactory(source));
            return set;
        }

        public static Dictionary<TKey, TValue> ReadL16Many<TKey, TValue>(this BinaryReader source, Func<BinaryReader, TKey> keyFactory, Func<BinaryReader, TValue> valueFactory, int offset = 0) => ReadTMany<TKey, TValue>(source, keyFactory, valueFactory, source.ReadUInt16(), offset);
        public static Dictionary<TKey, TValue> ReadL32Many<TKey, TValue>(this BinaryReader source, Func<BinaryReader, TKey> keyFactory, Func<BinaryReader, TValue> valueFactory, int offset = 0) => ReadTMany<TKey, TValue>(source, keyFactory, valueFactory, (int)source.ReadUInt32(), offset);
        public static Dictionary<TKey, TValue> ReadC32Many<TKey, TValue>(this BinaryReader source, Func<BinaryReader, TKey> keyFactory, Func<BinaryReader, TValue> valueFactory, int offset = 0) => ReadTMany<TKey, TValue>(source, keyFactory, valueFactory, (int)source.ReadCompressedUInt32(), offset);
        public static Dictionary<TKey, TValue> ReadTMany<TKey, TValue>(this BinaryReader source, Func<BinaryReader, TKey> keyFactory, Func<BinaryReader, TValue> valueFactory, int count, int offset = 0)
        {
            if (offset != 0) source.Skip(offset);
            var set = new Dictionary<TKey, TValue>();
            for (var i = 0; i < count; i++) set.Add(keyFactory(source), valueFactory(source));
            return set;
        }

        public static SortedDictionary<TKey, TValue> ReadL16SortedMany<TKey, TValue>(this BinaryReader source, int keySizeOf, Func<BinaryReader, TValue> valueFactory, int offset = 0) => ReadTSortedMany<TKey, TValue>(source, keySizeOf, valueFactory, source.ReadUInt16(), offset);
        public static SortedDictionary<TKey, TValue> ReadL32SortedMany<TKey, TValue>(this BinaryReader source, int keySizeOf, Func<BinaryReader, TValue> valueFactory, int offset = 0) => ReadTSortedMany<TKey, TValue>(source, keySizeOf, valueFactory, (int)source.ReadUInt32(), offset);
        public static SortedDictionary<TKey, TValue> ReadC32SortedMany<TKey, TValue>(this BinaryReader source, int keySizeOf, Func<BinaryReader, TValue> valueFactory, int offset = 0) => ReadTSortedMany<TKey, TValue>(source, keySizeOf, valueFactory, (int)source.ReadCompressedUInt32(), offset);
        public static SortedDictionary<TKey, TValue> ReadTSortedMany<TKey, TValue>(this BinaryReader source, int keySizeOf, Func<BinaryReader, TValue> valueFactory, int count, int offset = 0)
        {
            if (offset != 0) source.Skip(offset);
            var set = new SortedDictionary<TKey, TValue>();
            for (var i = 0; i < count; i++) set.Add(source.ReadT<TKey>(keySizeOf), valueFactory(source));
            return set;
        }

        public static bool ReadBool32(this BinaryReader source) => source.ReadUInt32() != 0;

        public static Vector2 ReadVector2(this BinaryReader source) => new Vector2(source.ReadSingle(), source.ReadSingle());
        public static Vector3 ReadVector3(this BinaryReader source) => new Vector3(source.ReadSingle(), source.ReadSingle(), source.ReadSingle());
        public static Vector4 ReadVector4(this BinaryReader source) => new Vector4(source.ReadSingle(), source.ReadSingle(), source.ReadSingle(), source.ReadSingle());
        /// <summary>
        /// Reads a column-major 3x3 matrix but returns a functionally equivalent 4x4 matrix.
        /// </summary>
        public static Matrix4x4 ReadColumnMajorMatrix3x3(this BinaryReader source)
        {
            var matrix = new Matrix4x4();
            for (var columnIndex = 0; columnIndex < 4; columnIndex++)
                for (var rowIndex = 0; rowIndex < 4; rowIndex++)
                {
                    // If we're in the 3x3 part of the matrix, read values. Otherwise, use the identity matrix.
                    if (rowIndex <= 2 && columnIndex <= 2) matrix.Set(rowIndex, columnIndex, source.ReadSingle());
                    else matrix.Set(rowIndex, columnIndex, rowIndex == columnIndex ? 1f : 0f);
                }
            return matrix;
        }
        /// <summary>
        /// Reads a row-major 3x3 matrix but returns a functionally equivalent 4x4 matrix.
        /// </summary>
        public static Matrix4x4 ReadRowMajorMatrix3x3(this BinaryReader source)
        {
            var matrix = new Matrix4x4();
            for (var rowIndex = 0; rowIndex < 4; rowIndex++)
                for (var columnIndex = 0; columnIndex < 4; columnIndex++)
                {
                    // If we're in the 3x3 part of the matrix, read values. Otherwise, use the identity matrix.
                    if (rowIndex <= 2 && columnIndex <= 2) matrix.Set(rowIndex, columnIndex, source.ReadSingle());
                    else matrix.Set(rowIndex, columnIndex, rowIndex == columnIndex ? 1f : 0f);
                }
            return matrix;
        }
        public static Matrix4x4 ReadColumnMajorMatrix4x4(this BinaryReader source)
        {
            var matrix = new Matrix4x4();
            for (var columnIndex = 0; columnIndex < 4; columnIndex++)
                for (var rowIndex = 0; rowIndex < 4; rowIndex++) matrix.Set(rowIndex, columnIndex, source.ReadSingle());
            return matrix;
        }
        public static Matrix4x4 ReadRowMajorMatrix4x4(this BinaryReader source)
        {
            var matrix = new Matrix4x4();
            for (var rowIndex = 0; rowIndex < 4; rowIndex++)
                for (var columnIndex = 0; columnIndex < 4; columnIndex++) matrix.Set(rowIndex, columnIndex, source.ReadSingle());
            return matrix;
        }
        public static Quaternion ReadQuaternionWFirst(this BinaryReader source)
        {
            var w = source.ReadSingle();
            var x = source.ReadSingle();
            var y = source.ReadSingle();
            var z = source.ReadSingle();
            return new Quaternion(x, y, z, w);
        }
        public static Quaternion ReadLEQuaternionWLast(this BinaryReader source)
        {
            var x = source.ReadSingle();
            var y = source.ReadSingle();
            var z = source.ReadSingle();
            var w = source.ReadSingle();
            return new Quaternion(x, y, z, w);
        }

        #endregion
    }
}