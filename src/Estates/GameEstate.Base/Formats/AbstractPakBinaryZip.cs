using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using static GameEstate.EstateDebug;

namespace GameEstate.Formats
{
    /// <summary>
    /// AbstractPakBinaryZip
    /// </summary>
    /// <seealso cref="GameEstate.Formats._Packages.PakBinary" />
    public abstract class AbstractPakBinaryZip : PakBinary
    {
        static readonly PropertyInfo ZipFile_KeyInfo = typeof(ZipFile).GetProperty("Key", BindingFlags.NonPublic | BindingFlags.Instance);

        public AbstractPakBinaryZip(byte[] key = null) => Key = key;

        readonly byte[] Key;

        protected abstract Func<BinaryReader, FileMetadata, EstatePakFile, Task<object>> GetObjectFactory(FileMetadata source);

        public override Task ReadAsync(BinaryPakFile source, BinaryReader r, ReadStage stage)
        {
            if (!(source is BinaryPakManyFile multiSource)) throw new NotSupportedException();
            if (stage != ReadStage.File) throw new ArgumentOutOfRangeException(nameof(stage), stage.ToString());

            source.UseBinaryReader = false;
            var files = multiSource.Files = new List<FileMetadata>();
            var pak = (ZipFile)(source.Tag = new ZipFile(r.BaseStream)); // { Key = Key });
            ZipFile_KeyInfo.SetValue(pak, Key);
            foreach (ZipEntry entry in pak)
            {
                var metadata = new FileMetadata
                {
                    Path = entry.Name.Replace('\\', '/'),
                    Crypted = entry.IsCrypted,
                    PackedSize = entry.CompressedSize,
                    FileSize = entry.Size,
                    Tag = entry,
                };
                metadata.ObjectFactory = GetObjectFactory(metadata);
                files.Add(metadata);
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
                using var input = pak.GetInputStream(entry);
                if (!input.CanRead) { Log($"Unable to read stream for file: {file.Path}"); exception?.Invoke(file, $"Unable to read stream for file: {file.Path}"); return Task.FromResult(System.IO.Stream.Null); }
                var s = new MemoryStream();
                input.CopyTo(s);
                return Task.FromResult((Stream)s);
            }
            catch (Exception e) { Log($"{file.Path} - Exception: {e.Message}"); exception?.Invoke(file, $"{file.Path} - Exception: {e.Message}"); return Task.FromResult(System.IO.Stream.Null); }
        }

        public override Task WriteDataAsync(BinaryPakFile source, BinaryWriter w, FileMetadata file, Stream data, Action<FileMetadata, string> exception = null)
        {
            var pak = (ZipFile)source.Tag;
            var entry = (ZipEntry)file.Tag;
            try
            {
                using var s = pak.GetInputStream(entry);
                data.CopyTo(s);
            }
            catch (Exception e) { exception?.Invoke(file, $"Exception: {e.Message}"); }
            return Task.CompletedTask;
        }
    }
}