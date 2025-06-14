using System.Text.Json;
using Domain.Shared;
using Dataset = Domain.Dataset;
using Scraper = Domain.Scraper;

namespace FormatConverter.Conversion;

internal class ToScraperConverter : BaseConverter
{
    public void Convert(string contestsFolder, string fileName)
    {
        IEnumerable<string> directories = Directory.EnumerateDirectories(contestsFolder);
        IEnumerable<Scraper.Contest> contests = directories.Select(ToScraperContest)
            .OrderBy(contest => contest.Year);

        string json = JsonSerializer.Serialize(contests);
        string filePath = Path.ChangeExtension(fileName, "json");
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        File.WriteAllText(filePath, json);
    }

    private Scraper.Contest ToScraperContest(string directory)
    {
        Dataset.Contest datasetContest = ReadJson<Dataset.Contest>(directory, CONTEST_FILE_NAME);
        int year = int.Parse(Path.GetFileName(directory));
        Scraper.Contestant[] contestants = ToScraperContestants(directory);
        Scraper.Round[] rounds = ToScraperRounds(directory);

        return new Scraper.Contest()
        {
            Year = year,
            Arena = datasetContest.Arena,
            City = datasetContest.City,
            Country = datasetContest.Country,
            IntendedCountry = datasetContest.IntendedCountry,
            Slogan = datasetContest.Slogan,
            LogoUrl = datasetContest.LogoUrl,
            Broadcasters = datasetContest.Broadcasters,
            Presenters = datasetContest.Presenters,
            Contestants = contestants,
            Rounds = rounds
        };
    }

    private Scraper.Contestant[] ToScraperContestants(string constestDirectory)
    {
        string contestantsPath = Path.Combine(constestDirectory, CONTESTANTS_FOLDER_NAME);
        IEnumerable<string> directories = Directory.EnumerateDirectories(contestantsPath).ToArray();

        return directories.Select(ToScraperContestant).ToArray();
    }

    private Scraper.Contestant ToScraperContestant(string directory)
    {
        Dataset.Contestant datasetContestant = ReadJson<Dataset.Contestant>(directory, CONTESTANT_FILE_NAME);
        string[] directoryData = Path.GetFileName(directory).Split(FILE_NAME_SEPARATOR);
        int id = int.Parse(directoryData[0]);
        string country = directoryData[1].ToUpper();
        Scraper.Lyrics[] lyrics = ToScraperLyrics(directory);

        return new Scraper.Contestant()
        {
            Id = id,
            Country = country,
            Artist = datasetContestant.Artist,
            Song = datasetContestant.Song,
            VideoUrls = datasetContestant.VideoUrls,
            Lyrics = lyrics,
            Bpm = datasetContestant.Bpm,
            Tone = datasetContestant.Tone,

            ArtistPeople = datasetContestant.ArtistPeople,
            Backings = datasetContestant.Backings,
            Dancers = datasetContestant.Dancers,
            StageDirector = datasetContestant.StageDirector,

            Composers = datasetContestant.Composers,
            Conductor = datasetContestant.Conductor,
            Lyricists = datasetContestant.Lyricists,
            Writers = datasetContestant.Writers,            
            
            Broadcaster = datasetContestant.Broadcaster,
            Commentators = datasetContestant.Commentators,
            Jury = datasetContestant.Jury,
            Spokesperson = datasetContestant.Spokesperson
        };
    }

    private Scraper.Lyrics[] ToScraperLyrics(string constestantDirectory)
    {
        string lyricsPath = Path.Combine(constestantDirectory, LYRICS_FOLDER_NAME);

        return Directory.EnumerateFiles(lyricsPath)
            .Select(FileToScraperLyrics)
            .ToArray();
    }

    private Scraper.Lyrics FileToScraperLyrics(string filePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string fileContent = File.ReadAllText(filePath);

        string[] fileNameData = fileName.Split(FILE_NAME_SEPARATOR);
        Scraper.LyricsType type = fileNameData[0] switch
        {
            "o" => Scraper.LyricsType.Original,
            "v" => Scraper.LyricsType.Version,
            _ => Scraper.LyricsType.Translation
        };
        string[] languages = fileNameData[1].Split(LANGUAGE_SEPARATOR);
        string[] displayedLanguages = fileNameData.Length > 2 
            ? fileNameData[2].Split(LANGUAGE_SEPARATOR)
            : null;

        fileContent = fileContent.Replace("\r", string.Empty);
        int splitIndex = fileContent.IndexOf("\n\n");
        string title = fileContent.Substring(0, splitIndex);
        string content = fileContent.Substring(splitIndex + LYRICS_PARTS_SEPARATOR.Length);

        return new Scraper.Lyrics()
        {
            Type = type,
            Languages = languages,
            DisplayedLanguages = displayedLanguages,
            Title = title,
            Content = content
        };
    }

    private Scraper.Round[] ToScraperRounds(string constestDirectory)
    {
        string roundsPath = Path.Combine(constestDirectory, ROUNDS_FOLDER_NAME);

        return Directory.EnumerateFiles(roundsPath)
            .Select(ToScraperRound)
            .ToArray();
    }

    private Scraper.Round ToScraperRound(string filePath)
    {
        string json = File.ReadAllText(filePath);
        Scraper.Round round = JsonSerializer.Deserialize<Scraper.Round>(json, SCRAPER_JSON_OPTIONS);
        round.Name = Path.GetFileNameWithoutExtension(filePath);

        return round;
    }

    private T ReadJson<T>(string directory, string fileName)
    {
        string json = File.ReadAllText(Path.Combine(directory, fileName + ".json"));

        return JsonSerializer.Deserialize<T>(json, DATASET_JSON_OPTIONS);
    }
}
