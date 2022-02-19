using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate.Formats
{
    /// <summary>
    /// PakBinaryZip
    /// </summary>
    /// <seealso cref="GameEstate.Formats._Packages.PakBinary" />
    public class PakBinaryZip : PakBinary
    {
        public static readonly PakBinary Instance = new PakBinaryZip(x => null, null);
        public PakBinaryZip(Func<string, Func<BinaryReader, FileMetadata, Task<object>>> objectFactory, byte[] key = null)
        {
            ObjectFactory = objectFactory;
            Key = key;
        }

        readonly Func<string, Func<BinaryReader, FileMetadata, Task<object>>> ObjectFactory;
        readonly byte[] Key;

        public override Task ReadAsync(BinaryPakFile source, BinaryReader r, ReadStage stage)
        {
            if (!(source is BinaryPakManyFile multiSource)) throw new NotSupportedException();
            if (stage != ReadStage.File) throw new ArgumentOutOfRangeException(nameof(stage), stage.ToString());

            source.UseBinaryReader = false;
            var files = multiSource.Files = new List<FileMetadata>();
            var pak = (ZipFile)(source.Tag = new ZipFile(r.BaseStream));
            pak.KeysRequired += (sender, e) => e.Key = Key;
            foreach (ZipEntry entry in pak)
            {
                var path = entry.Name.Replace('\\', '/');
                files.Add(new FileMetadata
                {
                    Path = path,
                    ObjectFactory = ObjectFactory(path),
                    Crypted = entry.IsCrypted,
                    PackedSize = entry.CompressedSize,
                    FileSize = entry.Size,
                    Tag = entry,
                });
            }
            return Task.CompletedTask;
        }

        public override Task WriteAsync(BinaryPakFile source, BinaryWriter w, WriteStage stage)
        {
            if (!(source is BinaryPakManyFile multiSource)) throw new NotSupportedException();

            source.UseBinaryReader = false;
            var files = multiSource.Files;
            var pak = (ZipFile)(source.Tag = new ZipFile(w.BaseStream));
            pak.KeysRequired += (sender, e) => e.Key = Key;
            pak.BeginUpdate();
            foreach (var file in files)
            {
                var entry = (ZipEntry)(file.Tag = new ZipEntry(Path.GetFileName(file.Path)));
                pak.Add(entry);
                source.PakBinary.WriteDataAsync(source, w, file, null, null);
            }
            pak.CommitUpdate();
            return Task.CompletedTask;
        }

        public override Task<Stream> ReadDataAsync(BinaryPakFile source, BinaryReader r, FileMetadata file, Action<FileMetadata, string> exception = null)
        {
            var pak = (ZipFile)source.Tag;
            var entry = (ZipEntry)file.Tag;
            try
            {
                using (var input = pak.GetInputStream(entry))
                {
                    if (!input.CanRead)
                    {
                        exception?.Invoke(file, $"Unable to read stream.");
                        return Task.FromResult(System.IO.Stream.Null);
                    }
                    var s = new MemoryStream();
                    input.CopyTo(s);
                    return Task.FromResult((Stream)s);
                }
            }
            catch (Exception e)
            {
                exception?.Invoke(file, $"Exception: {e.Message}");
                return Task.FromResult(System.IO.Stream.Null);
            }
        }

        public override Task WriteDataAsync(BinaryPakFile source, BinaryWriter w, FileMetadata file, Stream data, Action<FileMetadata, string> exception = null)
        {
            var pak = (ZipFile)source.Tag;
            var entry = (ZipEntry)file.Tag;
            try
            {
                using (var s = pak.GetInputStream(entry))
                    data.CopyTo(s);
            }
            catch (Exception e)
            {
                exception?.Invoke(file, $"Exception: {e.Message}");
            }
            return Task.CompletedTask;
        }
    }
}