using CommandLine;
using System;
using System.Threading.Tasks;
using static GameEstate.Estate;

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

            [Option("option", Default = DataOption.Stream | DataOption.Model, HelpText = "Data option")]
            public DataOption Option { get; set; }
        }

        static async Task<int> RunExportAsync(ExportOptions opts)
        {
            var from = ProgramState.Load(data => Convert.ToInt32(data), 0);
            var estate = EstateManager.GetEstate(opts.Estate);
            await ExportManager.ExportAsync(estate, estate.ParseResource(opts.Uri), opts.Path, from, opts.Option);
            ProgramState.Clear();
            return 0;
        }
    }
}