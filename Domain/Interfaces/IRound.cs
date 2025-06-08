namespace Domain.Interfaces;

public interface IRound<TPerformance> where TPerformance : IPerformance
{
    string Name { get; set; }
    DateOnly Date { get; set; }
    TimeOnly? Time { get; set; }
    int[] Disqualifieds { get; set; }
    IEnumerable<TPerformance> Performances { get; }
}
