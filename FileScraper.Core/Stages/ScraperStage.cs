using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileScraper.Core.Stages
{
    public class ScraperStage : IStage
    {
        private const string tag = "Scraper";

        private const string file_regex = "https?:\\/\\/cdn\\.discord(?:app)?\\.(?:com|net)\\/attachments\\/(?:\\d+)\\/(?:\\d+)\\/(?<filename>[^\\?\\s\"]+)(?<parameters>\\??[^\\s\"]*)";
        private const string link_regex = "(http|ftp|https)://([\\w_-]+(?:(?:\\.[\\w_-]+)+))([\\w.,@?^=%&:/~+#-]*[\\w@?^=%&/~+#-])?";

        public Task Run(FileScraperContext context)
        {
            if (!context.Options.OutputOptions.Links)
            {
                context.Logger.Debug(tag, "Not searching for links.");
            }
            else
            {
                context.Logger.Debug(tag, "Searching for links...");
                context.FoundLinks.AddRange(searchLinks(context));
            }
            
            context.FoundFiles.AddRange(searchFileUrls(context));

            return Task.CompletedTask;
        }

        private IEnumerable<string> searchLinks(FileScraperContext context)
        {
            var inputDirectory = new DirectoryInfo(context.Options.InputDirectory);

            foreach (var inputFile in inputDirectory.GetFiles())
            {
                var foundUrls = filterFileByPattern(context, inputFile.FullName, link_regex);

                foreach (var url in foundUrls)
                {
                    if (isValidUrl(url))
                    {
                        context.Logger.Debug(tag, $"Found url {url} in {inputFile}.");
                        yield return url;
                    }
                }
            }
            
            bool isValidUrl(string url)
            {
                const string discordLink = "cdn.discordapp.com";
                
                if (url.Contains(discordLink))
                    return false;
                
                if (context.Options.BlacklistedTerms.Any(url.Contains))
                    return false;
                
                return true;
            }
        }

        private IEnumerable<string> searchFileUrls(FileScraperContext context)
        {
            var inputDirectory = new DirectoryInfo(context.Options.InputDirectory);

            foreach (var inputFile in inputDirectory.GetFiles())
            {
                var foundFileUrls = filterFileByPattern(context, inputFile.FullName, file_regex);

                foreach (string url in foundFileUrls)
                {
                    if (isValidFileUrl(url))
                    {
                        context.Logger.Debug(tag, $"Found url {url} in {inputFile}.");
                        yield return url;
                    }
                    else
                    {
                        context.Logger.Debug(tag, $"Ignoring url {url} in {inputFile}.");
                    }
                }
            }

            bool isValidFileUrl(string url)
            {
                string extension = Path.GetExtension(url);

                if (!context.Options.OutputOptions.Extensions.Contains(extension))
                    return false;

                if (context.Options.BlacklistedTerms.Any(url.Contains))
                    return false;

                return true;
            }
        }

        private IEnumerable<string> filterFileByPattern(FileScraperContext context, string inputFile, string pattern)
        {
            using var streamReader = File.OpenText(inputFile);
            
            string line;
            while ((line = streamReader.ReadLine()) is not null)
            {
                var result = Regex.Match(line, pattern).Value;

                if (string.IsNullOrEmpty(result))
                    continue;

                context.Logger.Debug(tag, $"Found match in line {line}.");
                yield return result;
            }
        }
    }
}