using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using static GameEstate.EstateDebug;

namespace GameEstate.Formats
{
    /// <summary>
    /// PakBinaryZip2
    /// </summary>
    /// <seealso cref="GameEstate.Formats.PakBinary" />
    public class PakBinaryZip2 : PakBinary
    {
        readonly byte[] Key;
        readonly Func<FileMetadata, Func<BinaryReader, FileMetadata, EstatePakFile, Task<object>>> GetObjectFactory;

        public PakBinaryZip2(byte[] key = null, Func<FileMetadata, Func<BinaryReader, FileMetadata, EstatePakFile, Task<object>>> getObjectFactory = null)
        {
            Key = key;
            GetObjectFactory = getObjectFactory;
        }

        public override Task ReadAsync(BinaryPakFile source, BinaryReader r, ReadStage stage)
        {
            if (!(source is BinaryPakManyFile multiSource)) throw new NotSupportedException();
            if (stage != ReadStage.File) throw new ArgumentOutOfRangeException(nameof(stage), stage.ToString());

            source.UseBinaryReader = false;
            var files = multiSource.Files = new List<FileMetadata>();
            var pak = (ZipArchive)(source.Tag = new ZipArchive(r.BaseStream, ZipArchiveMode.Read));
            foreach (var entry in pak.Entries)
            {
                var metadata = new FileMetadata
                {
                    Path = entry.Name.Replace('\\', '/'),
                    PackedSize = entry.CompressedLength,
                    FileSize = entry.Length,
                    Tag = entry
                };
                metadata.ObjectFactory = GetObjectFactory(metadata);
                files.Add(metadata);
            }
            return Task.CompletedTask;
        }

        public override Task<Stream> ReadDataAsync(BinaryPakFile source, BinaryReader r, FileMetadata file, Action<FileMetadata, string> exception = null)
        {
            var pak = (ZipArchive)source.Tag;
            var entry = (ZipArchiveEntry)file.Tag;
            try
            {
                using var input = entry.Open();
                if (!input.CanRead) { Log($"Unable to read stream for file: {file.Path}"); exception?.Invoke(file, $"Unable to read stream for file: {file.Path}"); return Task.FromResult(System.IO.Stream.Null); }
                var s = new MemoryStream();
                input.CopyTo(s);
                return Task.FromResult((Stream)s);
            }
            catch (Exception e) { Log($"{file.Path} - Exception: {e.Message}"); exception?.Invoke(file, $"{file.Path} - Exception: {e.Message}"); return Task.FromResult(System.IO.Stream.Null); }
        }
    }
}