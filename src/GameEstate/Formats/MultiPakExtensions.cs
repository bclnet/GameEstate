using System;
using System.IO;
using System.Threading.Tasks;
using static GameEstate.Estate;

namespace GameEstate.Formats
{
    public static class MultiPakExtensions
    {
        const int MaxDegreeOfParallelism = 1; //8;

        #region Export / Import

        public static async Task ExportAsync(this BinaryPakFile source, string filePath, int from = 0, DataOption option = 0, Action<FileMetadata, int> advance = null, Action<FileMetadata, string> exception = null)
        {
            if (!(source is BinaryPakManyFile pak)) throw new NotSupportedException();

            // write pak
            if (!string.IsNullOrEmpty(filePath) && !Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

            // write files
            Parallel.For(from, pak.Files.Count, new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, async index =>
            {
                var file = pak.Files[index];
                var newPath = Path.Combine(filePath, file.Path);

                // create directory
                var directory = Path.GetDirectoryName(newPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory);

                // extract pak
                if (file.Pak != null) await file.Pak.ExportAsync(newPath);

                // extract file
                try
                {
                    await ExportFileAsync(file, pak, newPath, option, exception);
                    if (file.Parts != null && (option & DataOption.Raw) != 0)
                        foreach (var part in file.Parts) await ExportFileAsync(part, pak, Path.Combine(filePath, part.Path), option, exception);
                    advance?.Invoke(file, index);
                }
                catch (Exception e) { exception?.Invoke(file, $"Exception: {e.Message}"); }
            });

            // write pak-raw
            if ((option & DataOption.Marker) != 0) await new StreamPakFile(pak, null, source.Game, filePath).WriteAsync(null, PakBinary.WriteStage.File);
        }

        static async Task ExportFileAsync(FileMetadata file, BinaryPakManyFile pak, string newPath, DataOption option = 0, Action<FileMetadata, string> exception = null)
        {
            if (file.FileSize == 0 && file.PackedSize == 0) return;
            if ((option & file.DataOption) != 0)
            {
                if ((option & DataOption.Model) != 0)
                {
                    //var file = await pak.LoadFileObjectAsync<IUnknownFileModel>(file, unknownSource);
                    //using var b2 = await pak.LoadFileObjectAsync(file, option, exception);
                    return;
                }
                if ((option & DataOption.Transform) != 0)
                {
                    //using var b2 = await pak.LoadFileObjectAsync(file, option, exception);
                    return;
                }
            }
            using var b = await pak.LoadFileDataAsync(file, option, exception);
            using var s = new FileStream(newPath, FileMode.Create, FileAccess.Write);
            b.CopyTo(s);
            if (file.Parts != null && (option & DataOption.Raw) == 0)
                foreach (var part in file.Parts)
                {
                    using var b2 = await pak.LoadFileDataAsync(part, option, exception);
                    b2.CopyTo(s);
                }
        }

        public static async Task ImportAsync(this BinaryPakFile source, BinaryWriter w, string filePath, int from = 0, DataOption option = 0, Action<FileMetadata, int> advance = null, Action<FileMetadata, string> exception = null)
        {
            if (!(source is BinaryPakManyFile pak)) throw new NotSupportedException();

            // read pak
            if (string.IsNullOrEmpty(filePath) || !Directory.Exists(filePath)) { exception?.Invoke(null, $"Directory Missing: {filePath}"); return; }
            var setPath = Path.Combine(filePath, ".set");
            using (var r = new BinaryReader(File.Open(setPath, FileMode.Open, FileAccess.Read, FileShare.Read))) await PakBinary.Stream.ReadAsync(source, r, PakBinary.ReadStage._Set);
            var metaPath = Path.Combine(filePath, ".meta");
            using (var r = new BinaryReader(File.Open(setPath, FileMode.Open, FileAccess.Read, FileShare.Read))) await PakBinary.Stream.ReadAsync(source, r, PakBinary.ReadStage._Meta);
            var rawPath = Path.Combine(filePath, ".raw");
            if (File.Exists(rawPath)) using (var r = new BinaryReader(File.Open(rawPath, FileMode.Open, FileAccess.Read, FileShare.Read))) await PakBinary.Stream.ReadAsync(source, r, PakBinary.ReadStage._Raw);

            // write header
            if (from == 0) await source.PakBinary.WriteAsync(source, w, PakBinary.WriteStage.Header);

            // write files
            Parallel.For(0, pak.Files.Count, new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, async index =>
            {
                var file = pak.Files[index];
                var newPath = Path.Combine(filePath, file.Path);

                // check directory
                var directory = Path.GetDirectoryName(newPath);
                if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory)) { exception?.Invoke(file, $"Directory Missing: {directory}"); return; }

                // insert file
                try
                {
                    await source.PakBinary.WriteAsync(source, w, PakBinary.WriteStage.File);
                    using (var r = File.Open(newPath, FileMode.Open, FileAccess.Read, FileShare.Read)) await source.WriteFileDataAsync(w, file, new MemoryStream(r.ReadAllBytes()), option, exception);
                    advance?.Invoke(file, index);
                }
                catch (Exception e) { exception?.Invoke(file, $"Exception: {e.Message}"); }
            });
        }

        #endregion
    }
}