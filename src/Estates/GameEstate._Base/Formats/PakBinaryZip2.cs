using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace GameEstate.Formats
{
    /// <summary>
    /// PakBinaryZip2
    /// </summary>
    /// <seealso cref="GameEstate.Formats._Packages.PakBinary" />
    public class PakBinaryZip2 : PakBinary
    {
        public static readonly PakBinary Instance = new PakBinaryZip2();
        PakBinaryZip2() { }

        public override Task ReadAsync(BinaryPakFile source, BinaryReader r, ReadStage stage)
        {
            if (!(source is BinaryPakManyFile multiSource)) throw new NotSupportedException();
            if (stage != ReadStage.File) throw new ArgumentOutOfRangeException(nameof(stage), stage.ToString());

            source.UseBinaryReader = false;
            var files = multiSource.Files = new List<FileMetadata>();
            var pak = (ZipArchive)(source.Tag = new ZipArchive(r.BaseStream, ZipArchiveMode.Read));
            foreach (ZipArchiveEntry entry in pak.Entries)
                files.Add(new FileMetadata
                {
                    Path = entry.Name.Replace('\\', '/'),
                    PackedSize = entry.CompressedLength,
                    FileSize = entry.Length,
                    Tag = entry
                });
            return Task.CompletedTask;
        }

        public override Task<Stream> ReadDataAsync(BinaryPakFile source, BinaryReader r, FileMetadata file, Action<FileMetadata, string> exception = null)
        {
            var pak = (ZipArchive)source.Tag;
            var entry = (ZipArchiveEntry)file.Tag;
            try
            {
                using var input = entry.Open();
                if (!input.CanRead) { exception?.Invoke(file, $"Unable to read stream."); return Task.FromResult(System.IO.Stream.Null); }
                var s = new MemoryStream();
                input.CopyTo(s);
                return Task.FromResult((Stream)s);
            }
            catch (Exception e) { exception?.Invoke(file, $"Exception: {e.Message}"); return Task.FromResult(System.IO.Stream.Null); }
        }
    }
}