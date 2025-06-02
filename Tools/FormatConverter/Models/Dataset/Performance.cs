using FormatConverter.Models.Scraper;

namespace FormatConverter.Models.Dataset;

public class Performance : IPerformance<Score>
{
    public int ContestantId { get; set; }
    public int Running { get; set; }
    public int? Place { get; set; }
    public IEnumerable<Score> Scores { get; set; }
}
