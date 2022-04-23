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
        public static async Task ExportAsync(Estate estate, Resource resource, string filePath, int from, DataOption option)
        {
            using var pak = estate.OpenPakFile(resource);

            // export pak
            if (!(pak is MultiPakFile multiPak))
            {
                await ExportPakAsync(filePath, from, option, pak);
                return;
            }

            // write paks
            if ((option & DataOption.Marker) != 0)
            {
                if (!string.IsNullOrEmpty(filePath) && !Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
                var setPath = Path.Combine(filePath, ".set");
                using var w = new BinaryWriter(new FileStream(setPath, FileMode.Create, FileAccess.Write));
                await PakBinary.Stream.WriteAsync(new StreamPakFile(HttpHost.Factory, null, null, "Root")
                {
                    Files = multiPak.PakFiles.Select(x => new FileMetadata { Path = x.Name }).ToList()
                }, w, PakBinary.WriteStage._Set);
            }
            foreach (var _ in multiPak.PakFiles) await ExportPakAsync(filePath, from, option, _);
        }

        static async Task ExportPakAsync(string filePath, int from, DataOption option, EstatePakFile _)
        {
            if (!(_ is BinaryPakFile pak)) throw new InvalidOperationException("pak not a BinaryPakFile");
            var newPath = Path.Combine(filePath, Path.GetFileName(pak.FilePath));

            // write pak
            await pak.ExportAsync(newPath, from, option, (file, index) =>
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