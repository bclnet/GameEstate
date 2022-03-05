using GameEstate.Formats;
using System;
using System.IO;
using System.Threading.Tasks;
using static GameEstate.Estate;

namespace GameEstate.App
{
    public static class ImportManager
    {
        public static async Task ImportAsync(Estate estate, Resource resource, string filePath, int from)
        {
            foreach (var path in resource.Paths)
            {
                using var pak = estate.OpenPakFile(new[] { path }, resource.Game) as BinaryPakFile;
                if (pak == null)
                    throw new InvalidOperationException("Pak not a BinaryPakFile");
                using var w = new BinaryWriter(new FileStream(path, FileMode.Create, FileAccess.Write));
                await pak.ImportAsync(w, filePath, from, (file, index) =>
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