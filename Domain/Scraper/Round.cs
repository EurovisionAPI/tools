using Domain.Interfaces;
using Domain.Shared;

namespace Domain.Scraper;

public class Round : IRound
{
    public string Name { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? Time { get; set; }
    public int[] Disqualifieds { get; set; }
    public Performance[] Performances { get; set; }
}
