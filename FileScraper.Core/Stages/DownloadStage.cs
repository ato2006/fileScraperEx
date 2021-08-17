using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace FileScraper.Core.Stages
{
    public class DownloadStage : IStage
    {
        private const string tag = "Downloader";
        
        private readonly HttpClient _client = new();
        
        public async Task Run(FileScraperContext context)
        {
            context.Logger.Debug(tag, "Downloading files...");
            await downloadFoundFiles(context);
        }

        private async Task downloadFoundFiles(FileScraperContext context)
        {
            var foundFileUrls = context.FoundFiles ?? throw new ArgumentNullException(nameof(context.FoundFiles));
            int fileAmount = foundFileUrls.Count;
            
            string outputDirectory = context.Options.OutputDirectory;
            Directory.CreateDirectory(outputDirectory);
            
            context.Logger.Log(tag, $"Starting to download {foundFileUrls.Count} files...");

            for (int i = 0; i < foundFileUrls.Count; i++)
            {
                var fileUrl = foundFileUrls[i];
                string fileName = Path.GetFileName(fileUrl);

                context.Logger.Debug(tag, $"Downloading file {fileName} from {fileUrl}...");

                try
                {
                    await _client.DownloadFileAsync(fileUrl, Path.Combine(outputDirectory, fileName));
                }
                catch (Exception e)
                {
                    context.Logger.Error("Error", $"An error occurred with the message {e.Message}. Url: {fileUrl}");
                }

                context.Logger.Debug(tag, $"Finished downloading file {fileName}.");
                
                context.Logger.Log(tag, $"Downloaded {i} out of {fileAmount} files.");
            }
        }
    }
}