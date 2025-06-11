using Domain.Shared;

namespace Domain.Interfaces;

public interface IRound
{
    DateOnly Date { get; set; }
    TimeOnly? Time { get; set; }
    int[] Disqualifieds { get; set; }
    IEnumerable<Performance> Performances { get; }
}
