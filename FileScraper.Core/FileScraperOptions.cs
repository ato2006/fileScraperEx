using System;
using System.Collections.Generic;
using System.IO;

namespace FileScraper.Core
{
    public class FileScraperOptions
    {
        public FileScraperOptions(string inputDirectory, string outputDirectory)
        {
            InputDirectory = inputDirectory ?? throw new ArgumentNullException(nameof(inputDirectory));
            OutputDirectory = outputDirectory ?? throw new ArgumentNullException(nameof(outputDirectory));
        }

        public FileScraperOptions(string inputDirectory)
            : this(inputDirectory, Path.Combine(inputDirectory, "Scraped"))
        {
        }
        
        public string InputDirectory { get; }
        public string OutputDirectory { get; }
        public bool Verbose { get; set; }
        public ICollection<string> BlacklistedTerms { get; set; }
        public OutputOptions OutputOptions { get; } = new();

    }
}