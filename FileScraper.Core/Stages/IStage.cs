using System.Threading.Tasks;

namespace FileScraper.Core.Stages
{
    public interface IStage
    {
        Task Run(FileScraperContext context);
    }
}