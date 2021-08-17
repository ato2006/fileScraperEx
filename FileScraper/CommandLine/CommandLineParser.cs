using System;
using System.Collections.Generic;

namespace FileScraper.CommandLine
{
    public class CommandLineParser
    {
        private IDictionary<string, CommandLineSwitch> _flags = new Dictionary<string, CommandLineSwitch>();
        private IDictionary<string, CommandLineSwitch> _options = new Dictionary<string, CommandLineSwitch>();
        
        public void AddSwitch(CommandLineSwitch commandSwitch)
        {
            var collection = commandSwitch.HasParam
                ? _options
                : _flags;

            foreach (var alias in commandSwitch.Aliases)
                collection[alias] = commandSwitch;
        }
        
        public CommandParseResult Parse(string[] args)
        {
            var result = new CommandParseResult();

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                if (arg[0] == '-')
                {
                    var word = arg[1..];

                    if (_flags.TryGetValue(word, out var flag))
                    {
                        result.Flags.Add(flag);
                    }
                    else if (_options.TryGetValue(word, out var option))
                    {
                        result.Options[option] = args[i + 1];
                        i++;
                    }
                    else
                    {
                        throw new ArgumentException(nameof(args));
                    }
                }
                else if (result.InputDirectory == null)
                {
                    result.InputDirectory = arg.Replace("\"", "");
                }
                else
                {
                    throw new Exception("Too many input files specified.");
                }
            }

            return result;
        }
    }
}