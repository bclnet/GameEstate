using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RED.CRC32;
using GameEstate.Red.Formats.Red.Records;
using WolvenKit.Common.FNV1A;

namespace GameEstate.Red.Formats.Red.CR2W
{
    public class CR2WVerify : IDisposable
    {
        #region Fields
        private const uint MAGIC = 0x57325243;
        private const uint DEADBEEF = 0xDEADBEEF;

        private Stream m_stream;
        private byte[] _temp;
        private bool m_hasInternalBuffer;
        private string m_filePath;

        private CR2WFileHeader m_fileheader;
        private CR2WTable[] m_tableheaders;
        private Byte[] _strings;
        private CR2WName[] _names;
        private CR2WImport[] _imports;
        private CR2WProperty[] _table4;
        private CR2WExport[] _exports;
        private CR2WBuffer[] _buffers;
        private CR2WEmbedded[] _embedded;

        private bool isDisposed;
        private Dictionary<uint, string> m_dictionary;
        #endregion

        #region Constructor

        public static void VerifyFile(string path)
        {
            var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            var v = new CR2WVerify(fs, path);
        }

        internal CR2WVerify(Stream stream, string filePath)
        {
            m_stream = stream;
            m_filePath = filePath;
            m_stream.Seek(0, SeekOrigin.Begin);
            ProcessFile();
        }
        #endregion

        ~CR2WVerify()
        {
            Dispose(false);
        }

        #region Main
        private void ProcessFile()
        {
            var id = ReadStruct<uint>();
            if (id != MAGIC)
                throw new FormatException($"Not a CR2W file, Magic read as 0x{id:X8}");

            m_fileheader = ReadStruct<CR2WFileHeader>();
            if (m_fileheader.version > 163 || m_fileheader.version < 159)
                throw new FormatException($"Unknown Version {m_fileheader.version}. Supported versions: 159 - 163.");

            var dt = new CDateTime(m_fileheader.timeStamp, null, "");

            m_hasInternalBuffer = m_fileheader.bufferSize > m_fileheader.fileSize;
            m_tableheaders = ReadStructs<CR2WTable>(10);

            _strings = ReadStringsBuffer();
            _names = ReadTable<CR2WName>(1);
            _imports = ReadTable<CR2WImport>(2);
            _table4 = ReadTable<CR2WProperty>(3);
            _exports = ReadTable<CR2WExport>(4);
            _buffers = ReadTable<CR2WBuffer>(5);
            _embedded = ReadTable<CR2WEmbedded>(6);

            // Fixing
            for (int i = 0; i < _names.Length; i++)
                FixNameFNV1A(ref _names[i]);

            for (int i = 0; i < _exports.Length; i++)
                FixExportCRC32(ref _exports[i]);

            for (int i = 0; i < _buffers.Length; i++)
                FixBufferCRC32(ref _buffers[i]);

            // Write File
            m_stream.Seek(160, SeekOrigin.Begin);

            WriteStringBuffer();
            WriteTable<CR2WName>(_names, 1);
            WriteTable<CR2WImport>(_imports, 2);
            WriteTable<CR2WProperty>(_table4, 3);
            WriteTable<CR2WExport>(_exports, 4);
            WriteTable<CR2WBuffer>(_buffers, 5);
            WriteTable<CR2WEmbedded>(_embedded, 6);

            // Write Header again
            m_stream.Seek(0, SeekOrigin.Begin);

            m_fileheader.timeStamp = CDateTime.Now.ToUInt64();
            m_fileheader.crc32 = CalculateHeaderCRC32();

            WriteStruct<uint>(MAGIC);
            WriteStruct<CR2WFileHeader>(m_fileheader);
            WriteStructs<CR2WTable>(m_tableheaders);
        }
        #endregion

        #region Table Reading
        private byte[] ReadStringsBuffer()
        {
            var start = m_tableheaders[0].offset;
            var size = m_tableheaders[0].itemCount;
            var crc = m_tableheaders[0].crc32;

            _temp = new byte[size];
            m_stream.Read(_temp, 0, _temp.Length);

            m_dictionary = new Dictionary<uint, string>();
            StringBuilder sb = new StringBuilder();
            uint offset = 0;
            for (uint i = 0; i < size; i++)
            {
                var b = _temp[i];
                if (b == 0)
                {
                    m_dictionary.Add(offset, sb.ToString());
                    sb.Clear();
                    offset = i + 1;
                }
                else
                {
                    sb.Append((char)b);
                }
            }

            return _temp;
        }
        private void WriteStringBuffer()
        {
            m_tableheaders[0].crc32 = Crc32Algorithm.Compute(_strings);
            m_stream.Write(_strings, 0, _strings.Length);
        }

        private T[] ReadTable<T>(int index) where T : struct
        {
            m_stream.Seek(m_tableheaders[index].offset, SeekOrigin.Begin);

            var hash = new Crc32Algorithm(false);
            var table = ReadStructs<T>(m_tableheaders[index].itemCount, hash);

            return table;
        }
        private void WriteTable<T>(T[] array, int index) where T : struct
        {
            if (array.Length == 0)
                return;

            var crc = new Crc32Algorithm(false);
            WriteStructs<T>(array, crc);
            m_tableheaders[index].crc32 = crc.HashUInt32;
        }
        #endregion

        void FixNameFNV1A(ref CR2WName name)
        {
            var str = m_dictionary[name.value];
            var hash = FNV1A32HashAlgorithm.HashString(str, Encoding.ASCII, true);
            name.hash = hash;
        }
        void FixExportCRC32(ref CR2WExport export)
        {
            m_stream.Seek(export.dataOffset, SeekOrigin.Begin);
            _temp = new byte[export.dataSize];
            m_stream.Read(_temp, 0, _temp.Length);
            export.crc32 = Crc32Algorithm.Compute(_temp);
        }
        void FixBufferCRC32(ref CR2WBuffer buffer)
        {
            //This might throw errors, the way it should be checked for is by reading
            //the object tree to find the deferred data buffers that will point to a buffer.
            //The flag of the parent object indicates where to read the data from.
            //For now this is a crude workaround.
            if (m_hasInternalBuffer)
            {
                m_stream.Seek(buffer.offset, SeekOrigin.Begin);
                _temp = new byte[buffer.diskSize];
                m_stream.Read(_temp, 0, _temp.Length);
                buffer.crc32 = Crc32Algorithm.Compute(_temp);
            }
            else
            {
                var path = String.Format("{0}.{1}.buffer", m_filePath, buffer.index);
                if (!File.Exists(path))
                {
                    return;
                }
                _temp = File.ReadAllBytes(path);
                buffer.crc32 = Crc32Algorithm.Compute(_temp);
            }
        }
        private uint CalculateHeaderCRC32()
        {
            var hash = new Crc32Algorithm(false);
            hash.Append(BitConverter.GetBytes(MAGIC));
            hash.Append(BitConverter.GetBytes(m_fileheader.version));
            hash.Append(BitConverter.GetBytes(m_fileheader.flags));
            hash.Append(BitConverter.GetBytes(m_fileheader.timeStamp));
            hash.Append(BitConverter.GetBytes(m_fileheader.buildVersion));
            hash.Append(BitConverter.GetBytes(m_fileheader.fileSize));
            hash.Append(BitConverter.GetBytes(m_fileheader.bufferSize));
            hash.Append(BitConverter.GetBytes(DEADBEEF));
            hash.Append(BitConverter.GetBytes(m_fileheader.numChunks));
            foreach (var h in m_tableheaders)
            {
                hash.Append(BitConverter.GetBytes(h.offset));
                hash.Append(BitConverter.GetBytes(h.itemCount));
                hash.Append(BitConverter.GetBytes(h.crc32));
            }
            return hash.HashUInt32;
        }


        T ReadStruct<T>(Crc32Algorithm crc32 = null) where T : struct
        {
            var size = Marshal.SizeOf<T>();

            _temp = new byte[size];
            m_stream.Read(_temp, 0, size);

            var handle = GCHandle.Alloc(_temp, GCHandleType.Pinned);
            var item = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());

            if (crc32 != null)
                crc32.Append(_temp);

            handle.Free();

            return item;
        }
        T[] ReadStructs<T>(uint count, Crc32Algorithm crc32 = null) where T : struct
        {
            var size = Marshal.SizeOf<T>();
            var items = new T[count];

            _temp = new byte[size];
            for (uint i = 0; i < count; i++)
            {
                m_stream.Read(_temp, 0, size);

                var handle = GCHandle.Alloc(_temp, GCHandleType.Pinned);
                items[i] = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());

                if (crc32 != null)
                    crc32.Append(_temp);

                handle.Free();
            }

            return items;
        }
        void WriteStruct<T>(T value, Crc32Algorithm crc32 = null) where T : struct
        {
            _temp = new byte[Marshal.SizeOf<T>()];
            var handle = GCHandle.Alloc(_temp, GCHandleType.Pinned);

            Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), true);
            m_stream.Write(_temp, 0, _temp.Length);

            if (crc32 != null)
                crc32.Append(_temp);

            handle.Free();
        }
        void WriteStructs<T>(T[] array, Crc32Algorithm crc32 = null) where T : struct
        {
            var size = Marshal.SizeOf<T>();
            _temp = new byte[size];
            for (int i = 0; i < array.Length; i++)
            {
                var handle = GCHandle.Alloc(_temp, GCHandleType.Pinned);

                Marshal.StructureToPtr(array[i], handle.AddrOfPinnedObject(), true);
                m_stream.Write(_temp, 0, _temp.Length);

                if (crc32 != null)
                    crc32.Append(_temp);

                handle.Free();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;
            if (disposing)
                m_stream.Dispose();

            _strings = null;
            _names = null;
            _imports = null;
            _table4 = null;
            _exports = null;
            _buffers = null;
            _embedded = null;
            _temp = null;

            isDisposed = true;
        }
    }
}