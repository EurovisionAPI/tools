using System.Text;
using System.Text.Json;
using Domain.Scraper;

namespace Scanner;

internal class Program
{
    private static CountryCollection _countryCollection;

    static void Main(string[] args)
    {
        Properties.ReadArguments(args);
        _countryCollection = GetCountryCollection();

        string json = File.ReadAllText(Properties.INPUT_PATH);
        Contest[] contests = JsonSerializer.Deserialize<Contest[]>(json, JsonSerializerOptions.Web);
        IEnumerable<ContestScanResult> scanResults = contests.Select(ScanContest);
        WriteScanResult(scanResults);
    }

    private static CountryCollection GetCountryCollection()
    {
        string json = File.ReadAllText("countries.json");
        Dictionary<string, string> countries = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

        return new CountryCollection(countries);
    }

    private static ContestScanResult ScanContest(Contest contest)
    {
        ContestScanResult result = new ContestScanResult(contest.Year);

        if (string.IsNullOrEmpty(contest.Country))
            result.Add("Country");

        if (string.IsNullOrEmpty(contest.Arena))
            result.Add("Arena");

        if (string.IsNullOrEmpty(contest.City))
            result.Add("City");

        if (string.IsNullOrEmpty(contest.LogoUrl))
            result.Add("Logo url");

        if (contest.Contestants.IsNullOrEmpty())
            result.Add("Contestants");
        else
        {
            foreach (Contestant contestant in contest.Contestants)
            {
                UnavailableData unavailableData = ScanContestant(contest.Year, contestant);
                result.AddUnavailableContestant(unavailableData);
            }
        }

        if (contest.Rounds.IsNullOrEmpty())
            result.Add("Rounds");

        return result;
    }

    private static UnavailableData ScanContestant(int year, Contestant contestant)
    {
        UnavailableData result = new UnavailableData(contestant.Country);

        if (string.IsNullOrEmpty(contestant.Song))
            result.Add("Song title");

        if (string.IsNullOrEmpty(contestant.Artist))
            result.Add("Artist");

        if (contestant.ArtistPeople.IsNullOrEmpty())
            result.Add("Artist people");

        if (contestant.Lyrics.IsNullOrEmpty())
            result.Add("Lyrics");

        if (contestant.VideoUrls.IsNullOrEmpty())
            result.Add("Video");

        if (!contestant.Bpm.HasValue)
            result.Add("Bpm");

        if (string.IsNullOrEmpty(contestant.Tone))
            result.Add("Tone");

        if (!Properties.IS_JUNIOR)
        {
            // Last year with live orquestra
            if (year <= 1998 && string.IsNullOrEmpty(contestant.Conductor))
                result.Add("Conductor");
        }

        return result;
    }

    private static void WriteScanResult(IEnumerable<ContestScanResult> results)
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (ContestScanResult result in results)
        {
            if (result.HasUnavailableData)
            {
                ScanResultToString(stringBuilder, result);
            }
        }

        string startPattern = Properties.IS_JUNIOR ? "Junior" : "Senior";
        string text = File.ReadAllText(Properties.README_PATH);
        text.Replace(startPattern, startPattern + "\n" + stringBuilder.ToString());
        File.WriteAllText(Properties.README_PATH, text);
    }

    private static void ScanResultToString(StringBuilder stringBuilder, ContestScanResult result)
    {
        int tab = 0;

        void WriteLine(string text)
        {
            stringBuilder.Append(new string('\t', tab));
            stringBuilder.AppendLine(text);
        }

        WriteLine($"#### 📅 {result.Name}");
        tab++;

        foreach (string field in result.Fields)
            WriteLine($"- {field}");

        if (result.Contestants.Count > 0)
        {
            WriteLine($"- Contestants:");
            tab++;

            foreach (UnavailableData data in result.Contestants)
            {
                string countryCode = data.Name;
                string countryName = _countryCollection.GetCountryName(countryCode);
                string flagEmoji = countryCode.IsoCountryCodeToFlagEmoji();

                WriteLine($"- {flagEmoji} **{countryName}**");
                tab++;
                foreach (string field in data.Fields)
                    WriteLine($"- {field}");
                tab--;
            }

            tab--;
        }

        tab--;
    }
}
