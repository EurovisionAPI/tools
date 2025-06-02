using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using FormatConverter.Utilities;
using Dataset = FormatConverter.Models.Dataset;
using Scraper = FormatConverter.Models.Scraper;

namespace FormatConverter;

internal class Transformer
{
    private const string CONTESTS_FILE_NAME = "contest";
    private const string CONTESTANTS_FOLDER_NAME = "contestants";
    private const string CONTESTANT_FILE_NAME = "contestant";
    private const string LYRICS_FOLDER_NAME = "lyrics";
    private const string ROUNDS_FOLDER_NAME = "rounds";

    public void ToDatasetFormat(string contestsFile, string folderName)
    {
        Scraper.Contest[] contests = ReadContests(contestsFile);
        DirectoryInfo folder = Directory.CreateDirectory(folderName);

        foreach (Scraper.Contest contest in contests)
        {
            ToDatasetContest(folder, contest);
        }
    }
    private static Scraper.Contest[] ReadContests(string fileName)
    {
        JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        string json = File.ReadAllText(fileName);
        Scraper.Contest[] contests = JsonSerializer.Deserialize<Scraper.Contest[]>(json, serializerOptions);

        return contests;
    }

    private void ToDatasetContest(DirectoryInfo parentFolder, Scraper.Contest scraperContest)
    {
        DirectoryInfo folder = parentFolder.CreateSubdirectory(scraperContest.Year.ToString());
        Dataset.Contest datasetContest = new Dataset.Contest()
        {
            Year = scraperContest.Year,
            Arena = scraperContest.Arena,
            City = scraperContest.City,
            Country = scraperContest.Country,
            IntendedCountry = scraperContest.IntendedCountry,
            Slogan = scraperContest.Slogan,
            LogoUrl = scraperContest.LogoUrl,
            Presenters = scraperContest.Presenters,
            Broadcasters = scraperContest.Broadcasters
        };

        Save(datasetContest, folder, CONTESTS_FILE_NAME);
        ToDatasetContestants(folder, scraperContest.Contestants);
        StoreRounds(folder, scraperContest.Rounds);
    }

    private void ToDatasetContestants(DirectoryInfo parentFolder, Scraper.Contestant[] contestants)
    {
        DirectoryInfo folder = parentFolder.CreateSubdirectory(CONTESTANTS_FOLDER_NAME);

        foreach (Scraper.Contestant contestant in contestants)
        {
            StoreContestant(folder, contestant);
        }
    }

    private void StoreContestant(DirectoryInfo parentFolder, Scraper.Contestant contestant)
    {
        string folderName = $"{contestant.Id}_{contestant.Country.ToLower()}";
        DirectoryInfo folder = parentFolder.CreateSubdirectory(folderName);
        Dataset.Contestant contestantStore = new Dataset.Contestant()
        {
            Id = contestant.Id,
            Country = contestant.Country,
            Broadcaster = contestant.Broadcaster,
            Artist = contestant.Artist,
            ArtistPeople = contestant.ArtistPeople,
            Song = contestant.Song,
            VideoUrls = contestant.VideoUrls,
            Bpm = contestant.Bpm,
            Tone = contestant.Tone,
            Backings = contestant.Backings,
            Commentators = contestant.Commentators,
            Composers = contestant.Composers,
            Conductor = contestant.Conductor,
            Dancers = contestant.Dancers,
            Lyricists = contestant.Lyricists,
            Spokesperson = contestant.Spokesperson,
            StageDirector = contestant.StageDirector,
            Writers = contestant.Writers,
        };

        Save(contestantStore, folder, CONTESTANT_FILE_NAME);
        StoreLyrics(folder, contestant.Lyrics);
    }

    private void StoreLyrics(DirectoryInfo parentFolder, Scraper.Lyrics[] allLyrics)
    {
        DirectoryInfo folder = parentFolder.CreateSubdirectory(LYRICS_FOLDER_NAME);
        string[] fileNames = allLyrics.Select(GetLyricsFilename).ToArray();
        Dictionary<string, int> fileNameCounts = new Dictionary<string, int>();

        foreach (string fileName in fileNames)
        {
            if (fileNameCounts.TryGetValue(fileName, out int count))
                count = count == 0 ? 2 : count + 1;

            fileNameCounts[fileName] = count;
        }

        for (int i = allLyrics.Length - 1; i >= 0; i--)
        {
            Scraper.Lyrics lyrics = allLyrics[i];
            string fileName = fileNames[i];
            int count = fileNameCounts[fileName];

            if (count > 0)
            {
                count = fileNameCounts[fileName]--;
                fileName += $"_{count}";
            }

            StoreLyrics(folder, lyrics, fileName);
        }
    }

    private string GetLyricsFilename(Scraper.Lyrics lyrics)
    {
        string type = lyrics.Type switch
        {
            Scraper.LyricsType.Original => "o",
            Scraper.LyricsType.Version => "v",
            Scraper.LyricsType.Translation or _ => "t"
        };

        List<object> fileNameParts = new List<object>();

        if (lyrics.Languages != null && lyrics.Languages.Length > 0)
            fileNameParts.Add(string.Join(",", lyrics.Languages));

        if (lyrics.DisplayedLanguages != null && lyrics.DisplayedLanguages.Length > 0)
            fileNameParts.Add(string.Join(",", lyrics.DisplayedLanguages));

        return string.Join("_", fileNameParts);
    }

    private void StoreLyrics(DirectoryInfo folder, Scraper.Lyrics lyrics, string fileName)
    {
        string fileContent = $"{lyrics.Title}\n\n{lyrics.Content}";
        File.WriteAllText(Path.Combine(folder.FullName, fileName + ".txt"), fileContent);
    }

    private void StoreRounds(DirectoryInfo parentFolder, Scraper.Round[] rounds)
    {
        DirectoryInfo folder = parentFolder.CreateSubdirectory(ROUNDS_FOLDER_NAME);

        foreach (Scraper.Round round in rounds)
        {
            StoreRound(folder, round);
        }
    }

    private void StoreRound(DirectoryInfo folder, Scraper.Round round)
    {
        Dataset.Round roundStore = new Dataset.Round()
        {
            Date = round.Date,
            Time = round.Time,
            Disqualifieds = round.Disqualifieds,
            Performances = round.Performances?.Select(performance => new Dataset.Performance
            {
                ContestantId = performance.ContestantId,
                Running = performance.Running,
                Place = performance.Place,
                Scores = performance.Scores.Select(score => new Dataset.Score
                {
                    Name = score.Name,
                    Votes = score.Votes,
                })
            })
        };

        Save(roundStore, folder, round.Name);
    }

    private void Save<T>(T data, DirectoryInfo folder, string fileName)
    {
        JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, //JavaScriptEncoder.Create(UnicodeRanges.All),
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        string json = JsonSerializer.Serialize(data, serializerOptions);
        Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        File.WriteAllText(Path.Combine(folder.FullName, fileName + ".json"), json, encoding);
    }
}
