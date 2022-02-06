using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate.Formats
{
    public static class MultiPakExtensions
    {
        #region Export / Import

        public static async Task ExportAsync(this BinaryPakFile source, string filePath, int from = 0, Action<FileMetadata, int> advance = null, Action<FileMetadata, string> exception = null)
        {
            if (!(source is BinaryPakManyFile multiSource))
                throw new NotSupportedException();

            // write pak
            if (!string.IsNullOrEmpty(filePath) && !Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            // write files
            Parallel.For(from, multiSource.Files.Count, new ParallelOptions { /*MaxDegreeOfParallelism = 1*/ }, async index =>
            {
                var file = multiSource.Files[index];
                var newPath = Path.Combine(filePath, file.Path);

                // create directory
                var directory = Path.GetDirectoryName(newPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                // extract pak
                if (file.Pak != null)
                    await file.Pak.ExportAsync(newPath);

                // skip empty file
                if (file.FileSize == 0 && file.PackedSize == 0)
                {
                    advance?.Invoke(file, index);
                    return;
                }

                // extract file
                try
                {
                    using (var b = await multiSource.LoadFileDataAsync(file, exception))
                    using (var s = new FileStream(newPath, FileMode.Create, FileAccess.Write))
                        b.CopyTo(s);
                    advance?.Invoke(file, index);
                }
                catch (Exception e) { exception?.Invoke(file, $"Exception: {e.Message}"); }
            });

            // write pak-raw
            await new StreamPakFile(multiSource, null, source.Game, filePath).WriteAsync(null, PakBinary.WriteStage.File);

            //// write pak-raw
            //if (source.FilesRawSet != null && source.FilesRawSet.Count > 0)
            //    using (var w = new BinaryWriter(new FileStream(rawPath, FileMode.Create, FileAccess.Write)))
            //        await PakFormat.Stream.WriteAsync(source, w, PakFormat.WriteStage._Raw);
        }

        public static async Task ImportAsync(this BinaryPakFile source, BinaryWriter w, string filePath, int from = 0, Action<FileMetadata, int> advance = null, Action<FileMetadata, string> exception = null)
        {
            if (!(source is BinaryPakManyFile multiSource))
                throw new NotSupportedException();

            // read pak
            if (string.IsNullOrEmpty(filePath) || !Directory.Exists(filePath))
            {
                exception?.Invoke(null, $"Directory Missing: {filePath}");
                return;
            }
            var setPath = Path.Combine(filePath, ".set");
            using (var r = new BinaryReader(File.Open(setPath, FileMode.Open, FileAccess.Read, FileShare.Read)))
                await PakBinary.Stream.ReadAsync(source, r, PakBinary.ReadStage._Set);
            var metaPath = Path.Combine(filePath, ".meta");
            using (var r = new BinaryReader(File.Open(setPath, FileMode.Open, FileAccess.Read, FileShare.Read)))
                await PakBinary.Stream.ReadAsync(source, r, PakBinary.ReadStage._Meta);
            var rawPath = Path.Combine(filePath, ".raw");
            if (File.Exists(rawPath))
                using (var r = new BinaryReader(File.Open(rawPath, FileMode.Open, FileAccess.Read, FileShare.Read)))
                    await PakBinary.Stream.ReadAsync(source, r, PakBinary.ReadStage._Raw);

            // write header
            if (from == 0)
                await source.PakBinary.WriteAsync(source, w, PakBinary.WriteStage.Header);

            // write files
            Parallel.For(0, multiSource.Files.Count, new ParallelOptions { MaxDegreeOfParallelism = 1 }, async index =>
            {
                var file = multiSource.Files[index];
                var newPath = Path.Combine(filePath, file.Path);

                // check directory
                var directory = Path.GetDirectoryName(newPath);
                if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
                {
                    exception?.Invoke(file, $"Directory Missing: {directory}");
                    return;
                }

                // insert file
                try
                {
                    await source.PakBinary.WriteAsync(source, w, PakBinary.WriteStage.File);
                    using (var r = File.Open(newPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        await source.WriteFileDataAsync(w, file, new MemoryStream(r.ReadAllBytes()), exception);

                    advance?.Invoke(file, index);
                }
                catch (Exception e) { exception?.Invoke(file, $"Exception: {e.Message}"); }
            });
        }

        #endregion
    }
}