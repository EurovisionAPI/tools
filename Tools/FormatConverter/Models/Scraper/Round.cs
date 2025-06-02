namespace FormatConverter.Models.Scraper;

public class Round : IRound
{
    public string Name { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? Time { get; set; }
    public int[] Disqualifieds { get; set; }
    public IEnumerable<Performance> Performances { get; set; }
}
