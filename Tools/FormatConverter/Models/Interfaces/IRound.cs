namespace FormatConverter.Models.Scraper;

public interface IRound
{
    string Name { get; set; }
    DateOnly Date { get; set; }
    TimeOnly? Time { get; set; }
    int[] Disqualifieds { get; set; }
}
