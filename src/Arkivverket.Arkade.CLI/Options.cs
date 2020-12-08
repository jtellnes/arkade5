using CommandLine;

namespace Arkivverket.Arkade.CLI
{
    public abstract class Options
    {
        [Option('o', "output-directory", HelpText = "Directory to place Arkade output files.", Required = true)]
        public string OutputDirectory { get; set; }

        [Option('L', "language",
            HelpText =
                "Optional. Used to set language for files produced by Arkade.\n" +
                "Supported values are:\n" +
                "\t'en-GB' (British english) OR\n" +
                "\t'nb-NO' (Norsk bokmål)")]
        public string LanguageForOutputFiles { get; set; }
    }
}
