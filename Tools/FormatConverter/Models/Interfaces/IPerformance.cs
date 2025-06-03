namespace FormatConverter.Models.Scraper;

public interface IPerformance
{
    int ContestantId { get; set; }
    int Running { get; set; }
    int? Place { get; set; }
}

public interface IPerformance<TScore> : IPerformance where TScore : IScore
{
    IEnumerable<TScore> Scores { get; set; }
}
