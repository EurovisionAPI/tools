namespace Scanner;

internal class CountryCollection
{
    private Dictionary<string, string> _countries;

    public CountryCollection(Dictionary<string, string> countries)
    {
        _countries = countries;
    }

    public string GetCountryName(string countryCode)
    {
        return _countries[countryCode.ToUpper()];
    }
}
