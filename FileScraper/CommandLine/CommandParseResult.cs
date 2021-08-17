using System.Collections.Generic;

namespace FileScraper.CommandLine
{
    public class CommandParseResult
    {
        public Dictionary<CommandLineSwitch, string> Options { get; } = new();
        public List<CommandLineSwitch> Flags { get; } = new();
        public string InputDirectory { get; set; }

        public string GetOptionOrDefault(CommandLineSwitch option, string defaultValue)
        {
            if (!Options.TryGetValue(option, out string value))
                value = defaultValue;
            return value;
        }
    }
}