namespace FormatConverter.Models.Scraper;

public interface IPerformance<TScore> where TScore : IScore
{
    int ContestantId { get; set; }
    int Running { get; set; }
    int? Place { get; set; }
    IEnumerable<TScore> Scores { get; set; }
}
