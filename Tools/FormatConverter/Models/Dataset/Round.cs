using FormatConverter.Models.Scraper;

namespace FormatConverter.Models.Dataset;

public class Round : IRound<Performance>
{
    public string Name { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? Time { get; set; }
    public int[] Disqualifieds { get; set; }
    public IEnumerable<Performance> Performances { get; set; }
}
