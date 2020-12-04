using CommandLine;

namespace Arkivverket.Arkade.CLI
{
    public abstract class Options
    {
        [Option('o', "output-directory", HelpText = "Directory to place Arkade output files.", Required = true)]
        public string OutputDirectory { get; set; }

        [Option('L', "language", HelpText = "Optional. Used to set language for files produced by Arkade. Supported values are: 'en-GB' and 'nb-NO'")]
        public string LanguageForOutputFiles { get; set; }
    }
}
