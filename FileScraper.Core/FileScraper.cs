using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileScraper.Core
{
    using Logger;
    using Stages;
    
    public class FileScraper
    {
        private const string tag = "Scraper";

        private readonly IEnumerable<IStage> _stages = new IStage[]
        {
            new ScraperStage(),
            new DownloadStage(),
            new LinkStage()
        };
        
        public FileScraper(ILogger logger)
            => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        private ILogger _logger { get; }

        public async Task Scrape(FileScraperOptions options)
        {
            _logger.Log(tag,"Started file scraping.");

            var context = createFileScraperContext(options);
            await executeStages(context);
        }

        private FileScraperContext createFileScraperContext(FileScraperOptions options)
            => new(options, _logger);

        private async Task executeStages(FileScraperContext context)
        {
            foreach (var stage in _stages)
            {
                _logger.Log(tag, $"Executing {nameof(stage)}.");
                await stage.Run(context);
            }
        }
    }
}