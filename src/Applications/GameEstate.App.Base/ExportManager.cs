using GameEstate.Formats;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static GameEstate.Estate;

namespace GameEstate.App
{
    public static class ExportManager
    {
        public static async Task ExportAsync(Estate estate, Resource resource, string filePath, int from)
        {
            using var multiPak = estate.OpenPakFile(resource) as MultiPakFile;
            if (multiPak == null) throw new InvalidOperationException("multiPak not a MultiPakFile");
            // write paks header
            if (!string.IsNullOrEmpty(filePath) && !Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            var setPath = Path.Combine(filePath, ".set");
            using (var w = new BinaryWriter(new FileStream(setPath, FileMode.Create, FileAccess.Write)))
                await PakBinary.Stream.WriteAsync(new StreamPakFile(HttpHost.Factory, null, null, "Root") { Files = multiPak.PakFiles.Select(x => new FileMetadata { Path = x.Name }).ToList() }, w, PakBinary.WriteStage._Set);

            // write paks
            foreach (var p in multiPak.PakFiles)
            {
                if (!(p is BinaryPakFile pak)) throw new InvalidOperationException("multiPak not a BinaryPakFile");
                var newPath = Path.Combine(filePath, Path.GetFileName(pak.FilePath));

                await pak.ExportAsync(newPath, from, (file, index) =>
                {
                    //if ((index % 50) == 0)
                    //    Console.WriteLine($"{file.Path}");
                }, (file, message) =>
                {
                    Console.WriteLine($"{message}: {file?.Path}");
                });
            }
        }
    }
}