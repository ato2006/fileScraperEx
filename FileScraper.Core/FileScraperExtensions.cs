using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace FileScraper.Core
{
    public static class FileScraperExtensions
    {
        private static readonly Random _random = new();
        public static async Task DownloadFileAsync(this HttpClient client, string url, string outputFile)
        {
            var fileBytes = await client.GetByteArrayAsync(url);

            if (File.Exists(outputFile)) 
                outputFile = getNonBlockingFileName(outputFile);

            await File.WriteAllBytesAsync(outputFile, fileBytes);
        }

        private static string getNonBlockingFileName(string file)
        {
            string extensionlessFile = Path.GetFileNameWithoutExtension(file);
            string extension = Path.GetExtension(file);
            string newFileName = extensionlessFile + $"_{_random.Next(0, 6000)}" + extension;

            return newFileName;
        }
    }
}