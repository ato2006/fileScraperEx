using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileScraper.Core.Stages
{
    public class LinkStage : IStage
    {
        public Task Run(FileScraperContext context)
        {
            saveLinks(context);

            return Task.CompletedTask;
        }

        private static void saveLinks(FileScraperContext context)
        {
            if (!context.Options.OutputOptions.Links)
                return;
            
            var foundLinks = context.FoundLinks ?? throw new ArgumentNullException(nameof(context.FoundLinks));
            string outputFile = Path.Combine(context.Options.OutputDirectory, "FoundLinks.txt");
            
            var linkBuffer = new StringBuilder();

            foreach (var link in foundLinks)
                linkBuffer.Append(link + "\n");
            
            File.AppendAllText(outputFile, linkBuffer.ToString());
        }
    }
}