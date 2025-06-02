namespace FormatConverter.Models.Scraper;

public interface IScore
{
    string Name { get; set; }
    Dictionary<string, int> Votes { get; set; }
}
