﻿using CommandLine;
using System;
using System.Threading.Tasks;
using static GameEstate.Estate;

namespace GameEstate.App.Cli
{
    partial class Program
    {
        [Verb("import", HelpText = "Insert files contents to pak.")]
        class ImportOptions
        {
            [Option('e', "estate", Required = true, HelpText = "Estate")]
            public string Estate { get; set; }

            [Option('u', "uri", Required = true, HelpText = "Pak file to be created")]
            public Uri Uri { get; set; }

            [Option("path", Default = @".\out", HelpText = "Insert folder")]
            public string Path { get; set; }

            [Option("option", Default = 0, HelpText = "Data option")]
            public DataOption Option { get; set; }
        }

        static async Task<int> RunImportAsync(ImportOptions opts)
        {
            var from = ProgramState.Load(data => Convert.ToInt32(data), 0);
            var estate = EstateManager.GetEstate(opts.Estate);
            await ImportManager.ImportAsync(estate, estate.ParseResource(opts.Uri), opts.Path, from, opts.Option);
            ProgramState.Clear();
            return 0;
        }
    }
}