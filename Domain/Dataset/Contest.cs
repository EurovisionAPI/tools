using Domain.Interfaces;

namespace Domain.Dataset;

public class Contest : IContest
{
    public string Arena { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string IntendedCountry { get; set; }
    public string Slogan { get; set; }
    public string[] Broadcasters { get; set; }
    public string[] Presenters { get; set; }
}
