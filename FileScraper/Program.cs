using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FileScraper
{
    using CommandLine;
    using Core;
    using Core.Logger;
    
    class Program
    {
        private static async Task Main(string[] args)
        {
            printAbout();

            var consoleLogger = new FilteredLogger(new ConsoleLogger());

            var parser = new CommandLineParser();
            foreach(var commandSwitch in CommandLineSwitches.AllSwitches)
                parser.AddSwitch(commandSwitch);

            try
            {
                var result = parser.Parse(args);
                var options = getScraperOptions(result);

                if (result.Flags.Contains(CommandLineSwitches.Help))
                {
                    printInstructions();
                }
                else
                {
                    consoleLogger.IncludeDebug = result.Flags.Contains(CommandLineSwitches.VerboseOutput);

                    var fileScraper = new FileScraper(consoleLogger);
                    await fileScraper.Scrape(options);
                }
                
                Console.WriteLine(options.Verbose);
            }
            catch (Exception e)
            {
                consoleLogger.Error("Error", $"An error occurred with the message {e.Message}.");
            }
        }

        private static void printAbout()
        {
#if DEBUG
            Console.WriteLine("FileScraper (DEBUG)");
#else
            Console.WriteLine("FileScraper");
#endif

            Console.WriteLine("Scrapes all important information from Discord messages.");
        }

        private static void printInstructions()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("   FileScraper.exe [options] <input-dir> [options]");
            Console.WriteLine();
        }

        private static FileScraperOptions getScraperOptions(CommandParseResult result)
        {
            string inputDirectory = result.InputDirectory ?? throw new ArgumentNullException(nameof(result));

            if (!Directory.Exists(inputDirectory))
                throw new ArgumentException(nameof(inputDirectory));

            string outputDirectory = result.GetOptionOrDefault(CommandLineSwitches.OutputDirectory,
                Path.Combine(inputDirectory, "Scraped"));

            var extensions = readOptionFile(result, CommandLineSwitches.FileExtensions, "Extensions.txt");
            var blacklistedTerms = readOptionFile(result, CommandLineSwitches.FileExtensions, "Blacklist.txt");
            
            var options = new FileScraperOptions(inputDirectory, outputDirectory)
            {
                OutputOptions =
                {
                    Extensions = extensions.ToList().AsReadOnly(),
                    Links = result.Flags.Contains(CommandLineSwitches.Links),
                },
                BlacklistedTerms = blacklistedTerms.ToList().AsReadOnly(),
                Verbose = result.Flags.Contains(CommandLineSwitches.VerboseOutput)
            };

            return options;
        }

        private static IEnumerable<string> readOptionFile(CommandParseResult result, CommandLineSwitch commandSwitch, string fileName)
        {
            string workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string optionFile = result.GetOptionOrDefault(commandSwitch,
                Path.Combine(workingDirectory!, fileName));

            var terms = File.ReadAllLines(optionFile);

            return terms;
        }
    }
}
