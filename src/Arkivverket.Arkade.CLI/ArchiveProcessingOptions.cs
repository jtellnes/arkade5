using CommandLine;

namespace Arkivverket.Arkade.CLI
{
    public abstract class ArchiveProcessingOptions : Options
    {
        [Option('a', "archive", HelpText = "Archive directory or file (.tar) to process.", Required = true)]
        public string Archive { get; set; }

        [Option('t', "type", HelpText = "Archive type, valid values: noark3, noark5 or fagsystem", Required = true)]
        public string ArchiveType { get; set; }

        [Option('p', "processing-area", HelpText = "Directory to place temporary files and logs.", Required = true)]
        public string ProcessingArea { get; set; }
    }
}
