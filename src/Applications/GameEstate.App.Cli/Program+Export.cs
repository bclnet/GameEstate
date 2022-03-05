using CommandLine;
using System;
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
            await ExportManager.ExportAsync(estate, estate.ParseResource(opts.Uri), opts.Path, from);
            ProgramState.Clear();
            return 0;
        }
    }
}