namespace Domain.Interfaces;

public interface IContest
{
    int Year { get; set; }
    string Arena { get; set; }
    string City { get; set; }
    string Country { get; set; }
    string IntendedCountry { get; set; }
    string Slogan { get; set; }
    string LogoUrl { get; set; }
    string[] Presenters { get; set; }
    string[] Broadcasters { get; set; }
}
