using CommandLine;
using GameEstate.Formats;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GameEstate.App.Cli
{
    partial class Program
    {
        [Verb("export", HelpText = "Extract files contents to folder.")]
        class ExportOptions
        {
            [Option('e', "estate", Required = true, HelpText = "Estate")]
            public string Estate { get; set; }

            [Option('u', "uri", Required = true, HelpText = "Pak file to be extracted")]
            public Uri Uri { get; set; }

            [Option("path", Default = @".\out", HelpText = "Output folder")]
            public string Path { get; set; }

            [Option("fix", Default = false, HelpText = "Fix")]
            public bool Fix { get; set; }
        }

        static async Task<int> RunExportAsync(ExportOptions opts)
        {
            var from = ProgramState.Load(data => Convert.ToInt32(data), 0);
            var estate = EstateManager.GetEstate(opts.Estate);

            using var multiPak = estate.OpenPakFile(estate.ParseResource(opts.Uri)) as MultiPakFile;
            if (multiPak == null)
                throw new InvalidOperationException("multiPak not a MultiPakFile");
            // write paks header
            var filePath = opts.Path;
            if (!string.IsNullOrEmpty(filePath) && !Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            var setPath = Path.Combine(filePath, ".set");
            using (var w = new BinaryWriter(new FileStream(setPath, FileMode.Create, FileAccess.Write)))
                await PakBinary.Stream.WriteAsync(new StreamPakFile(HttpHost.Factory, null, null, "Root") { Files = multiPak.PakFiles.Select(x => new FileMetadata { Path = x.Name }).ToList() }, w, PakBinary.WriteStage._Set);

            // write paks
            foreach (var p in multiPak.PakFiles)
            {
                if (p is not BinaryPakFile pak)
                    throw new InvalidOperationException("multiPak not a BinaryPakFile");
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
            ProgramState.Clear();
            return 0;
        }
    }
}