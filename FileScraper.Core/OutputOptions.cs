using System.Collections.Generic;

namespace FileScraper.Core
{
    public class OutputOptions
    {
        public ICollection<string> Extensions { get; set; }
        public bool Links { get; set; }
    }
}