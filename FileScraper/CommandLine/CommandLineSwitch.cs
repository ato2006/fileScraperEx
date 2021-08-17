using System.Collections.Generic;
using System.Linq;

namespace FileScraper.CommandLine
{
    public class CommandLineSwitch
    {
        public string Description { get; }
        public ICollection<string> Aliases { get; }
        public bool HasParam { get; }
        public CommandLineSwitch(IEnumerable<string> aliases, string description)
        {
            Aliases = aliases.ToList().AsReadOnly();
            Description = description;
            
            CommandLineSwitches.AllSwitches.Add(this);
        }

        public CommandLineSwitch(IEnumerable<string> aliases, string description, bool hasParam)
            : this(aliases, description)
        {
            HasParam = hasParam;
        }
    }
}