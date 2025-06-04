using System.Diagnostics.Metrics;
using System.Text.Json;
using Dataset = Domain.Dataset;
using Scraper = Domain.Scraper;

namespace FormatConverter.Conversion;

internal class ToScraperConverter : BaseConverter
{
    public void Convert(string contestsFolder, string fileName)
    {
        IEnumerable<Scraper.Contest> contests = Directory.EnumerateDirectories(contestsFolder)
            .Select(ToScraperContest);
    }

    private Scraper.Contest ToScraperContest(string directory)
    {
        string contestPath = Path.Combine(directory, CONTEST_FILE_NAME);
        Dataset.Contest datasetContest = ReadJson<Dataset.Contest>(contestPath);
        int year = int.Parse(Path.GetDirectoryName(directory));
        IEnumerable<Scraper.Contestant> contestants = ToScraperContestants(directory);
        IEnumerable<Scraper.Round> rounds = ToScraperRounds(directory);

        return new Scraper.Contest()
        {
            Year = year,
            Arena = datasetContest.Arena,
            City = datasetContest.City,
            Country = datasetContest.Country,
            IntendedCountry = datasetContest.IntendedCountry,
            Slogan = datasetContest.Slogan,
            LogoUrl = datasetContest.LogoUrl,
            Presenters = datasetContest.Presenters,
            Broadcasters = datasetContest.Broadcasters,
            Contestants = contestants,
            Rounds = rounds
        };
    }

    private IEnumerable<Scraper.Contestant> ToScraperContestants(string constestDirectory)
    {
        string contestantsPath = Path.Combine(constestDirectory, CONTESTANTS_FOLDER_NAME);

        return Directory.EnumerateDirectories(contestantsPath)
            .Select(ToScraperContestant);
    }

    private Scraper.Contestant ToScraperContestant(string directory)
    {
        string contestantPath = Path.Combine(directory, CONTESTANT_FILE_NAME);
        Dataset.Contestant datasetContestant = ReadJson<Dataset.Contestant>(contestantPath);
        string directoryName = Path.GetFileName(directory);
        int id = int.Parse(directoryName);
        string country = directoryName;
        IEnumerable<Scraper.Lyrics> lyrics = ToScraperLyrics(directory);

        return new Scraper.Contestant()
        {
            Id = id,
            Country = country,
            Artist = datasetContestant.Artist,
            ArtistPeople = datasetContestant.ArtistPeople,
            Song = datasetContestant.Song,
            Lyrics = lyrics,
            VideoUrls = datasetContestant.VideoUrls,
            Dancers = datasetContestant.Dancers,
            Backings = datasetContestant.Backings,
            Composers = datasetContestant.Composers,
            Lyricists = datasetContestant.Lyricists,
            Writers = datasetContestant.Writers,
            Conductor = datasetContestant.Conductor,
            StageDirector = datasetContestant.StageDirector,
            Bpm = datasetContestant.Bpm,
            Tone = datasetContestant.Tone,
            Broadcaster = datasetContestant.Broadcaster,
            Spokesperson = datasetContestant.Spokesperson,
            Commentators = datasetContestant.Commentators
        };
    }

    private IEnumerable<Scraper.Lyrics> ToScraperLyrics(string constestantDirectory)
    {
        string lyricsPath = Path.Combine(constestantDirectory, LYRICS_FOLDER_NAME);

        return Directory.EnumerateDirectories(contestantsPath)
            .Select(ToScraperRound);
    }

    private IEnumerable<Scraper.Round> ToScraperRounds(string constestDirectory)
    {
        string contestantsPath = Path.Combine(constestDirectory, ROUNDS_FOLDER_NAME);

        return Directory.EnumerateDirectories(contestantsPath)
            .Select(ToScraperRound);
    }

    private T ReadJson<T>(string file)
    {
        string json = File.ReadAllText(file);

        return JsonSerializer.Deserialize<T>(json, DATASET_JSON_OPTIONS);
    }
}
