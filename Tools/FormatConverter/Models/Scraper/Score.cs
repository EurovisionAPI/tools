namespace FormatConverter.Models.Scraper;

public class Score : IScore
{
    public string Name { get; set; }
    public int Points { get; set; }
    public Dictionary<string, int> Votes { get; set; }
}
