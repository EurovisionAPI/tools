using Domain.Scraper;
using Microsoft.VisualBasic;

namespace Scanner;

internal class Program
{
    static void Main(string[] args)
    {
        Properties.ReadArguments(args);

        Contest[] contests;
    }

    private static void ScanContest(Contest contest)
    {
        UnavailableData unavailable = new UnavailableData(contest.Year.ToString());

        if (string.IsNullOrEmpty(contest.Country))
            unavailable.Add("Country");

        if (string.IsNullOrEmpty(contest.Arena))
            unavailable.Add("Arena");

        if (string.IsNullOrEmpty(contest.City))
            unavailable.Add("City");

        if (string.IsNullOrEmpty(contest.LogoUrl))
            unavailable.Add("Logo url");

        if (contest.Contestants.IsNullOrEmpty())
            unavailable.Add("Contestants");
        else
        {
            foreach (Contestant contestant in contest.Contestants)
            {
                string countryName = CountryCollection.GetCountryName(contestant.Country);
                GetLogUnavailableData(CheckUnvailableData, contestant, $"Contestant {countryName}:", unavailable);
            }
        }

        if (contest.Rounds.IsNullOrEmpty())
            unavailable.Add("Rounds");
    }

    private static void ScanContestant(Contestant contestant)
    {
        if (string.IsNullOrEmpty(contestant.Song))
            unavailable.Add("Song title");

        if (string.IsNullOrEmpty(contestant.Artist))
            unavailable.Add("Artist");

        if (contestant.ArtistPeople.IsNullOrEmpty())
            unavailable.Add("Artist people");

        if (contestant.Lyrics.IsNullOrEmpty())
            unavailable.Add("Lyrics");

        if (contestant.VideoUrls.IsNullOrEmpty())
            unavailable.Add("Video");

        if (!contestant.Bpm.HasValue)
            unavailable.Add("Bpm");

        if (string.IsNullOrEmpty(contestant.Tone))
            unavailable.Add("Tone");

        if(senior)
        {
            if (year <= 1998) // Last year with live orquestra

            if (string.IsNullOrEmpty(contestant.Conductor))
                noAvailable.Add("Conductor");
        }
    }

    private static void ScanContests(IEnumerable)
}
