using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P4KLib
{
    public class P4KVFS : IDisposable
    {
        public Func<byte[], byte[]> DecryptFunc { get; set; }
        private bool Logging { get; set; } = false;
        private string Filename { get; set; }

        private FileStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private Mutex mutex;

        public P4KVFS(Func<byte[], byte[]> decryptionConversionCallback)
            => DecryptFunc = decryptionConversionCallback;

        public void Dispose()
        {
            mutex.WaitOne();
            if (stream != null) stream.Close();
        }

        public string Filepath => stream?.Name;

        public Mutex GetStream(out FileStream stream, out BinaryReader reader, out BinaryWriter writer)
        {
            mutex.WaitOne();
            stream = this.stream;
            reader = this.reader;
            writer = this.writer;
            return mutex;
        }

        private OrderedDictionary<string, P4KFile> Files = new OrderedDictionary<string, P4KFile>();
        //private List<CentralDirectory> Directory = new List<CentralDirectory>();
        //private List<FileStructure> FileStructures = new List<FileStructure>();
        private CentralDirectoryEnd central_directory_end;
        private CentralDirectoryLocatorOffset central_directory_locator_offset;
        private CentralDirectoryLocator central_directory_locator;

        public P4KFile this[string index] => Files[index];

        public P4KFile this[int index] => Files[index];

        public int Count => Files.Count;

        public bool ReadOnly { get; private set; }

        public void Initialize(string filename, bool readOnly, bool logging = false)
        {
            Filename = filename;
            Logging = logging;

            mutex = new Mutex();

            if (readOnly) { ReadOnly = true; stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read); }
            else
            {
                try { ReadOnly = false; stream = File.Open(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.None); }
                catch (IOException) { ReadOnly = true; stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read); }
            }

            try
            {
                mutex.WaitOne();

                reader = new BinaryReader(stream);
                writer = ReadOnly ? null : new BinaryWriter(stream);

                var offset_central_directory_end = FindEndCentralDirectoryOffset(stream, reader);
                central_directory_end = ReadPK(stream, reader, offset_central_directory_end) as CentralDirectoryEnd;

                var offset_central_directory_locator_offset = offset_central_directory_end - 0x14;
                central_directory_locator_offset = ReadPK(stream, reader, offset_central_directory_locator_offset) as CentralDirectoryLocatorOffset;

                var offset_central_directory_locator = central_directory_locator_offset.DirectoryLocatorOffset;
                central_directory_locator = ReadPK(stream, reader, offset_central_directory_locator) as CentralDirectoryLocator;

                PopulateDirectory(stream, reader, central_directory_locator);
            }
            finally { mutex.ReleaseMutex(); }

            if (Logging) Console.WriteLine($"Finished loading {filename}");
        }

        public void WriteContentDirectoryChunk(FileStream stream, BinaryReader reader, BinaryWriter writer)
        {
            stream.Position = central_directory_locator.ContentDirectoryOffset;

            var cd_chunk = CreateCentralDirectoryChunk();

            writer.Write(cd_chunk);

            central_directory_locator.ContentDictionarySize = cd_chunk.Length;
            central_directory_locator.DontentDictionaryCount = Files.Count;
            central_directory_locator.ContentDictionaryCount2 = Files.Count;

            central_directory_locator_offset.DirectoryLocatorOffset = stream.Position;
            var locator_bytes = central_directory_locator.CreateBinaryData(true);
            var locator_offset_bytes = central_directory_locator_offset.CreateBinaryData(true);
            var cd_end_bytes = central_directory_end.CreateBinaryData(true);

            writer.Write(locator_bytes);
            writer.Write(locator_offset_bytes);
            writer.Write(cd_end_bytes);
        }

        public void AllocateFilesystemChunk(long allocation_size, out long file_offset, out long total_allocated_size, bool regenerate_content_directory = true, byte fill = 0)
        {
            var current_offset = central_directory_locator.ContentDirectoryOffset;

            var total_allocation = 0L;

            // Just rebase the existing offset to be always 0x1000 aligned for simplicity
            if (current_offset % 0x1000 != 0) total_allocation += 0x1000 - (current_offset % 0x1000);

            var start_offset = current_offset + total_allocation;
            var allocation_padded = allocation_size;

            // make 4kb aligned allocation
            if (allocation_padded % 0x1000 != 0) allocation_padded += 0x1000 - (allocation_padded % 0x1000);

            total_allocation += allocation_padded;

            var mutex = GetStream(out var stream, out var reader, out var writer);

            var total_data_to_move = stream.Length - current_offset;

            stream.Position = current_offset;

            var chunks = new List<byte[]>();
            var data_to_read = total_data_to_move;
            while (data_to_read > 0)
            {
                var iteration_bytes = Math.Min(0xFFFFFFFL, data_to_read);
                chunks.Add(reader.ReadBytes((int)iteration_bytes));
                data_to_read -= iteration_bytes;
            }

            stream.SetLength(stream.Length + total_allocation);
            stream.Position = current_offset;
            for (var i = 0; i < total_allocation; i++) stream.WriteByte(0x00);

            stream.Position = start_offset;
            for (var i = 0; i < allocation_size; i++) stream.WriteByte(fill);

            central_directory_locator.ContentDirectoryOffset += total_allocation;

            if (regenerate_content_directory) WriteContentDirectoryChunk(stream, reader, writer);

            mutex.ReleaseMutex();

            file_offset = start_offset;
            total_allocated_size = total_allocation;
        }

        public byte[] CreateCentralDirectoryChunk()
        {
            using var cd_stream = new MemoryStream();
            using var cd_writer = new BinaryWriter(cd_stream);
            for (var i = 0; i < Files.Count; i++)
            {
                var file = Files[i];
                var bytes = file.centralDirectory.CreateBinaryData(true);
                cd_writer.Write(bytes);
            }

            return cd_stream.ToArray();
        }

        //public delegate void PopulateDirectoryCallback(int current_index, int content_dictionary_count);
        //public PopulateDirectoryCallback PopulateDirectoryCallbackFunc = null;

        public volatile int current_index = 0;
        public volatile int content_dictionary_count = 0;

        void PopulateDirectory(FileStream stream, BinaryReader reader, CentralDirectoryLocator central_directory_locator)
        {
            long central_directory_base_offset = central_directory_locator.ContentDirectoryOffset;

            stream.Position = central_directory_base_offset;
            var central_directory_data = reader.ReadBytes(central_directory_locator.ContentDictionarySize);
            using var central_directory_stream = new MemoryStream(central_directory_data);
            using var central_directory_reader = new BinaryReader(central_directory_stream);
            // rebase current offset to 0 because we've copied data to a relative stream
            var central_directory_current_offset = 0L;

            content_dictionary_count = central_directory_locator.DontentDictionaryCount;
            for (var i = 0; i < central_directory_locator.DontentDictionaryCount; i++)
            {
                var central_directory = ReadPK(central_directory_stream, central_directory_reader, central_directory_current_offset) as CentralDirectory;
                central_directory_current_offset = central_directory_stream.Position;
                //TODO async query
                //var file_structure = ReadPK(stream, reader, central_directory.extra.data_offset) as FileStructure;
                Files[central_directory.Filename] = new P4KFile(this, central_directory);
                //var file_data = ReadDataFromFileSection(stream, reader, file_offset.extra.file_data_offset, file_offset.extra.file_data_length);
                // if(PopulateDirectoryCallbackFunc != null) PopulateDirectoryCallbackFunc(i, central_directory_locator.content_dictionary_count);
                current_index = i;
            }
        }

        public object ReadPK(Stream stream, BinaryReader reader, Int64 offset)
        {
            stream.Seek(offset, SeekOrigin.Begin);

            var magic = reader.ReadInt16();
            if (magic != 0x4B50) throw new Exception("Invalid PK offset");

            var signature = (Signature)reader.ReadInt16();

            switch (signature)
            {
                case Signature.CentralDirectory:
                    var centralDirectory = new CentralDirectory(stream, reader);
                    if (Logging) Console.WriteLine($"Found CentralDirectory {centralDirectory.Filename}");
                    if (Logging) Console.WriteLine($"Searching for FileStructure @{centralDirectory.Extra.data_offset.ToString("X")}");
                    return centralDirectory;
                case Signature.FileStructure:
                    var fileStructure = new FileStructure(stream, reader);
                    if (Logging) Console.WriteLine($"Found FileStructure {fileStructure.Filename}");
                    return fileStructure;
                case Signature.CentralDirectoryLocator:
                    var centralDirectoryLocator = new CentralDirectoryLocator(stream, reader);
                    if (Logging) Console.WriteLine($"Found CentralDirectoryLocator @{offset}");
                    return centralDirectoryLocator;
                case Signature.CentralDirectoryLocatorOffset:
                    var centralDirectoryLocatorOffset = new CentralDirectoryLocatorOffset(stream, reader);
                    if (Logging) Console.WriteLine($"Found CentralDirectoryLocatorOffset @{offset}");
                    return centralDirectoryLocatorOffset;
                case Signature.CentralDirectoryEnd:
                    var centralDirectoryEnd = new CentralDirectoryEnd(stream, reader);
                    if (Logging) Console.WriteLine($"Found CentralDirectoryEnd @{offset}");
                    return centralDirectoryEnd;
                default: throw new NotImplementedException();
            }
        }

        static long FindEndCentralDirectoryOffset(FileStream stream, BinaryReader reader)
        {
            // last PK must be within 0x1000 alignment, worst case scenario
            var length = Math.Min(stream.Length, 0x2000L);
            stream.Position = stream.Length - length;
            var data = reader.ReadBytes((int)length);

            byte[] end_central_directory_magic = {
                0x50, 0x4B, 0x05, 0x06, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF
            };

            var current_offset = length - end_central_directory_magic.LongLength;
            var found = true;
            for (; current_offset > 0; current_offset--)
            {
                found = true;
                for (var magic_index = 0L; magic_index < end_central_directory_magic.LongLength; magic_index++)
                    if (data[current_offset + magic_index] != end_central_directory_magic[magic_index]) { found = false; break; }
                if (found) break;
            }

            if (!found) throw new Exception("Couldn't find CentralDirectoryEnd");

            var offset_from_eof = length - current_offset;
            stream.Position = stream.Length - offset_from_eof;
            return stream.Position;
        }

        public static byte[] ReadDataFromFileSection(FileStream stream, BinaryReader reader, long offset, int size)
        {
            stream.Position = offset;
            if (offset % 0x1000 != 0) throw new Exception("File data alignment error");
            return reader.ReadBytes(size);
        }

        public void AddP4KFile(P4KFile file, string filename, byte[] data, ref CentralDirectory central_directory, ref FileStructure file_structure)
        {
            if (!Files.ContainsValue(file))
            {
                file.centralDirectory = new CentralDirectory(filename, data);
                file.FileStructure = new FileStructure(filename, data);

                // Add this first otherwise the Locator count wont be correct
                Files[file.Filepath] = file;

                AllocateFilesystemChunk(data.Length + 0x1000, out var allocation_offset, out var total_allocated_size, false);// add an extra +0x1000 chunk in for the fileinfo

                file.centralDirectory.Extra.data_offset = allocation_offset;
                var crypt = new SHA256Managed();
                file.centralDirectory.Extra.sha256_hash = crypt.ComputeHash(data);

                var mutex = this.GetStream(out var stream, out var reader, out var writer);

                // these are the same for some reason... weird
                file.FileStructure.Extra.DataOffset = file.centralDirectory.Extra.data_offset;

                // write fileinfo
                stream.Position = allocation_offset;
                file.FileStructure.WriteBinaryToStream(stream, writer, true);

                // not actually part of the structure, just a useful helper to get the data as its
                // immediately after this structure
                file.FileStructure.Extra.FileDataOffset = stream.Position;
                file.centralDirectory.Extra.compressed_file_length = file.FileStructure.Extra.CompressedFileLength;
                file.centralDirectory.Extra.uncompressed_file_length = file.FileStructure.Extra.UncompressedFileLength;

                writer.Write(data);

                WriteContentDirectoryChunk(stream, reader, writer);

                mutex.ReleaseMutex();
            }
        }

        public bool FilepathExists(string filename)
            => Files.Contains(filename);
    }
}
