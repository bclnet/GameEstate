using CommandLine;
using System;
using System.Threading.Tasks;

namespace GameEstate.App.Cli
{
    [Verb("dev", HelpText = "Extract files contents to folder.")]
    class TestOptions
    {
        [Option('e', "estate", HelpText = "Estate")]
        public string Estate { get; set; }

        [Option('u', "uri", HelpText = "Pak file to be extracted")]
        public Uri Uri { get; set; }
    }

    partial class Program
    {
        static async Task<int> RunTestAsync(TestOptions opts)
        {
            await new EstateTest().TestAsync();
            return 0;
        }
    }
}