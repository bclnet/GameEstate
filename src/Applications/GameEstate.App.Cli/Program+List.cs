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
        [Verb("list", HelpText = "Extract files contents to folder.")]
        class ListOptions
        {
            [Option('e', "estate", HelpText = "Estate")]
            public string Estate { get; set; }

            [Option('u', "uri", HelpText = "Pak file to be extracted")]
            public Uri Uri { get; set; }
        }

        static Task<int> RunListAsync(ListOptions opts)
        {
            // list estates
            if (string.IsNullOrEmpty(opts.Estate))
            {
                Console.WriteLine("Estates installed:\n");
                foreach (var estate2 in EstateManager.Estates)
                    Console.WriteLine($"{estate2.Key} - {estate2.Value.Name}");
                return Task.FromResult(0);
            }

            var estate = EstateManager.GetEstate(opts.Estate);
            // list found locations in estate
            if (opts.Uri == null)
            {
                var estateGames = string.Join(", ", estate.Games.Values);
                Console.WriteLine($"{estate.Name}\n{estate.Description}\nGames: {estateGames}\n");
                var locations = estate.FileManager.Locations;
                if (locations.Count == 0)
                {
                    Console.WriteLine("No games found.");
                    return Task.FromResult(0);
                }
                Console.WriteLine("Locations found:\n");
                foreach (var location in locations)
                {
                    var (name, description) = estate.GetGame(location.Key);
                    Console.WriteLine($"{description} - {location.Value}");
                }
                return Task.FromResult(0);
            }
            // list files in pack for estate
            else
            {
                Console.WriteLine($"{estate.Name} - {opts.Uri}\n");
                //if (estate.OpenPakFile(estate.ParseResource(opts.Uri)) is not MultiPakFile multiPak)
                //    throw new InvalidOperationException("multiPak not a MultiPakFile");
                using var multiPak = estate.OpenPakFile(estate.ParseResource(opts.Uri)) as MultiPakFile ?? throw new InvalidOperationException("multiPak not a MultiPakFile");
                if (multiPak.PakFiles.Count == 0)
                {
                    Console.WriteLine("No paks found.");
                    return Task.FromResult(0);
                }
                Console.WriteLine("Paks found:");
                foreach (var p in multiPak.PakFiles)
                {
                    if (p is not BinaryPakManyFile pak)
                        throw new InvalidOperationException("multiPak not a BinaryPakFile");
                    Console.WriteLine($"\n{pak.Name}");
                    foreach (var exts in pak.Files.Select(x => Path.GetExtension(x.Path)).GroupBy(x => x))
                        Console.WriteLine($"  files{exts.Key}: {exts.Count()}");
                }
            }
            return Task.FromResult(0);
        }
    }
}