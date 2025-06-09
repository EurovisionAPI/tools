using System.Text;
using System.Text.Json;
using Domain.Scraper;

namespace Scanner;

internal class Program
{
    private static CountryCollection _countryCollection;

    static void Main(string[] args)
    {
        Console.WriteLine("Arguments: [{0}]", string.Join(", ", args));
        Properties.ReadArguments(args);
        Console.WriteLine("Input path: " + Properties.INPUT_PATH);

        _countryCollection = GetCountryCollection();

        string juniorResult = ScanContests(Properties.JUNIOR_FILENAME, true);
        string seniorResult = ScanContests(Properties.SENIOR_FILENAME, false);
        WriteResult(juniorResult, seniorResult);
    }

    private static T ReadJson<T>(string fileName)
    {
        fileName = Path.ChangeExtension(fileName, "json");
        string filePath = Path.Combine(Properties.INPUT_PATH, fileName);
        Console.WriteLine("Read Json: " + filePath);
        string json = File.ReadAllText(fileName);

        return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions.Web);
    }

    private static CountryCollection GetCountryCollection()
    {
        string fileName = Properties.COUNTRIES_FILENAME;
        Dictionary<string, string> countries = ReadJson<Dictionary<string, string>>(fileName);

        return new CountryCollection(countries);
    }

    private static string ScanContests(string fileName, bool isJunior)
    {
        Contest[] contests = ReadJson<Contest[]>(fileName);
        IEnumerable<ContestScanResult> scanResults = contests.Select(contest => ScanContest(contest, isJunior));
        return ToString(scanResults);
    }

    private static ContestScanResult ScanContest(Contest contest, bool isJunior)
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
                UnavailableData unavailableData = ScanContestant(contest.Year, isJunior, contestant);
                result.AddUnavailableContestant(unavailableData);
            }
        }

        if (contest.Rounds.IsNullOrEmpty())
            result.Add("Rounds");

        return result;
    }

    private static UnavailableData ScanContestant(int year, bool isJunior, Contestant contestant)
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

        if (!isJunior)
        {
            // Last year with live orquestra
            if (year <= 1998 && string.IsNullOrEmpty(contestant.Conductor))
                result.Add("Conductor");
        }

        return result;
    }

    private static string ToString(IEnumerable<ContestScanResult> results)
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (ContestScanResult result in results)
        {
            if (result.HasUnavailableData)
            {
                WriteScanResult(stringBuilder, result);
            }
        }

        return stringBuilder.ToString();
    }

    private static void WriteScanResult(StringBuilder stringBuilder, ContestScanResult result)
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

    private static void WriteResult(string junior, string senior)
    {
        string text = File.ReadAllText(Properties.README_PATH);
        text.Replace(Properties.JUNIOR_PATTERN, $"{Properties.JUNIOR_PATTERN}\n{junior}");
        text.Replace(Properties.SENIOR_PATTERN, $"{Properties.SENIOR_PATTERN}\n{senior}");
        File.WriteAllText(Properties.README_PATH, text);
    }
}
