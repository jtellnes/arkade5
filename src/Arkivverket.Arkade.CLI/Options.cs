using CommandLine;

namespace Arkivverket.Arkade.CLI
{
    public abstract class Options
    {
        [Option('o', "output-directory", HelpText = "Directory to place Arkade output files.", Required = true)]
        public string OutputDirectory { get; set; }

        [Option('l', "language",
            HelpText =
                "Optional. Used to set language for files produced by Arkade.\n" +
                "Supported values are:\n" +
                "\t'en' (British English) OR\n" +
                "\t'nb' (Norwegian Bokmål)")]
        public string LanguageForOutputFiles { get; set; }
    }
}
