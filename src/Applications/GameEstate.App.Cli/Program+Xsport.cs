using CommandLine;
using GameEstate.Formats;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate.App.Cli
{
    partial class Program
    {
        [Verb("xsport", HelpText = "Insert files contents to pak.")]
        class XsportOptions
        {
            [Option('e', "estate", Required = true, HelpText = "Estate")]
            public string Estate { get; set; }

            [Option('u', "uri", Required = true, HelpText = "Pak file to be created")]
            public Uri Uri { get; set; }

            [Option("path", Default = @".\out", HelpText = "Insert folder")]
            public string Path { get; set; }
        }

        static async Task<int> RunXsportAsync(XsportOptions opts)
        {
            var from = ProgramState.Load(data => Convert.ToInt32(data), 0);
            var estate = EstateManager.GetEstate(opts.Estate);
            var resource = estate.ParseResource(opts.Uri);
            foreach (var path in resource.Paths)
            {
                using var pak = estate.OpenPakFile(new[] { path }, resource.Game) as BinaryPakFile ?? throw new InvalidOperationException("Pak not a BinaryPakFile");
                using var w = new BinaryWriter(new FileStream(path, FileMode.Create, FileAccess.Write));
                await pak.ImportAsync(w, opts.Path, from, (file, index) =>
                {
                    //if ((index % 50) == 0)
                    //Console.WriteLine($"{file.Path}");
                }, (file, message) =>
                {
                    Console.WriteLine($"{message}: {file.Path}");
                });
            }
            ProgramState.Clear();
            return 0;
        }
    }
}