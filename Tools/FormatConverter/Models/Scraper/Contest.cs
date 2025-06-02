using FormatConverter.Models.Interfaces;

namespace FormatConverter.Models.Scraper;

public class Contest : IContest
{
    public int Year { get; set; }
    public string Arena { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string IntendedCountry { get; set; }
    public string Slogan { get; set; }
    public string LogoUrl { get; set; }
    public string[] Presenters { get; set; }
    public string[] Broadcasters { get; set; }
    public Contestant[] Contestants { get; set; }
    public Round[] Rounds { get; set; }
}
