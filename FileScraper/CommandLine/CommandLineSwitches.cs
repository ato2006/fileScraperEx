using System.Collections.Generic;

namespace FileScraper.CommandLine
{
    public static class CommandLineSwitches
    {
        public static readonly List<CommandLineSwitch> AllSwitches = new();
        
        public static readonly CommandLineSwitch Help = new(new[]
        {
            "h", "help"
        }, "Shows help.");
        
        public static readonly CommandLineSwitch VerboseOutput = new(new[]
        {
            "v", "verbose"
        }, "Enable verbose output. Useful for debugging purposes.");

        public static readonly CommandLineSwitch FileExtensions = new(new[]
        {
            "e", "extensions"
        }, "File extensions that are scraped for.", true);

        public static readonly CommandLineSwitch Links = new(new[]
        {
            "l", "link"
        }, "Include all found links in the output.");
        
        public static readonly CommandLineSwitch OutputDirectory = new(new[]
        {
            "o", "output"
        }, "Set output directory of the file scraper.", true);

        public static readonly CommandLineSwitch BlacklistedTerms = new(new[]
        {
            "b", "blacklist"
        }, "Does not scrape files or urls that contain specific terms.", true);

    }
}