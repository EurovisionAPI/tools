namespace Domain.Shared;

public class Performance
{
    public int ContestantId { get; set; }
    public int Running { get; set; }
    public int? Place { get; set; }
    public IEnumerable<Score> Scores { get; set; }
}
