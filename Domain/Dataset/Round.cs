using Domain.Interfaces;
using Domain.Shared;

namespace Domain.Dataset;

public class Round : IRound
{
    public DateOnly Date { get; set; }
    public TimeOnly? Time { get; set; }
    public int[] Disqualifieds { get; set; }
    public Performance[] Performances { get; set; }
}
