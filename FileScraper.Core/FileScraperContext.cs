using System;
using System.Collections.Generic;

namespace FileScraper.Core
{
    using Logger;

    public class FileScraperContext
    {
        public FileScraperOptions Options { get; }
        public ILogger Logger { get; }

        public List<string> FoundLinks { get; } = new();
        public List<string> FoundFiles { get; } = new();
        
        public FileScraperContext(FileScraperOptions options, ILogger logger)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}