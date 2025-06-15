using System.Data;
using System.Text.Json;
using Domain.Shared;
using Dataset = Domain.Dataset;
using Scraper = Domain.Scraper;

namespace FormatConverter.Conversion;

internal class ToDatasetConverter : BaseConverter
{
    public async Task ConvertAsync(string contestsFile, string folderName)
    {
        DirectoryInfo folder = Directory.CreateDirectory(folderName);
        using FileStream file = File.Open(contestsFile, FileMode.Open);
        var contests = JsonSerializer.DeserializeAsyncEnumerable<Scraper.Contest>(file, SCRAPER_JSON_OPTIONS);

        await foreach (Scraper.Contest contest in contests)
        {
            ToDatasetContest(folder, contest);
        }
    }

    private void ToDatasetContest(DirectoryInfo parentFolder, Scraper.Contest scraperContest)
    {
        DirectoryInfo folder = parentFolder.CreateSubdirectory(scraperContest.Year.ToString());
        Dataset.Contest datasetContest = new Dataset.Contest()
        {
            Arena = scraperContest.Arena,
            City = scraperContest.City,
            Country = scraperContest.Country,
            IntendedCountry = scraperContest.IntendedCountry,
            Slogan = scraperContest.Slogan,
            Presenters = scraperContest.Presenters,
            Broadcasters = scraperContest.Broadcasters
        };

        Save(datasetContest, folder, CONTEST_FILE_NAME);
        ToDatasetContestants(folder, scraperContest.Contestants);
        StoreRounds(folder, scraperContest.Rounds);
    }

    private void ToDatasetContestants(DirectoryInfo parentFolder, IEnumerable<Scraper.Contestant> contestants)
    {
        DirectoryInfo folder = parentFolder.CreateSubdirectory(CONTESTANTS_FOLDER_NAME);

        foreach (Scraper.Contestant contestant in contestants)
        {
            StoreContestant(folder, contestant);
        }
    }

    private void StoreContestant(DirectoryInfo parentFolder, Scraper.Contestant contestant)
    {
        string folderName = contestant.Id + FILE_NAME_SEPARATOR + contestant.Country.ToLower();
        DirectoryInfo folder = parentFolder.CreateSubdirectory(folderName);
        Dataset.Contestant contestantStore = new Dataset.Contestant()
        {
            Id = contestant.Id,
            Country = contestant.Country,
            Artist = contestant.Artist,
            Song = contestant.Song,
            VideoUrls = contestant.VideoUrls,
            Bpm = contestant.Bpm,
            Tone = contestant.Tone,

            ArtistPeople = contestant.ArtistPeople,
            Backings = contestant.Backings,
            Dancers = contestant.Dancers,
            StageDirector = contestant.StageDirector,

            Composers = contestant.Composers,
            Conductor = contestant.Conductor,
            Lyricists = contestant.Lyricists,
            Writers = contestant.Writers,

            Broadcaster = contestant.Broadcaster,          
            Commentators = contestant.Commentators,
            Jury = contestant.Jury,
            Spokesperson = contestant.Spokesperson
        };

        Save(contestantStore, folder, CONTESTANT_FILE_NAME);
        StoreLyrics(folder, contestant.Lyrics.ToArray());
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
                fileName += FILE_NAME_SEPARATOR + count;
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

        List<object> fileNameParts = [type];

        if (lyrics.Languages != null && lyrics.Languages.Length > 0)
            fileNameParts.Add(string.Join(LANGUAGE_SEPARATOR, lyrics.Languages));

        if (lyrics.DisplayedLanguages != null && lyrics.DisplayedLanguages.Length > 0)
            fileNameParts.Add(string.Join(LANGUAGE_SEPARATOR, lyrics.DisplayedLanguages));

        return string.Join(FILE_NAME_SEPARATOR, fileNameParts);
    }

    private void StoreLyrics(DirectoryInfo folder, Scraper.Lyrics lyrics, string fileName)
    {
        string fileContent = lyrics.Title + LYRICS_PARTS_SEPARATOR + lyrics.Content;
        File.WriteAllText(Path.Combine(folder.FullName, fileName + ".txt"), fileContent);
    }

    private void StoreRounds(DirectoryInfo parentFolder, IEnumerable<Scraper.Round> rounds)
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
            Performances = round.Performances?.Select(performance => new Performance
            {
                ContestantId = performance.ContestantId,
                Running = performance.Running,
                Place = performance.Place,
                Scores = performance.Scores.Select(score => new Score
                {
                    Name = score.Name,
                    Points = score.Points,
                    Votes = score.Votes,
                })
            }).ToArray()
        };

        Save(roundStore, folder, round.Name);
    }

    private void Save<T>(T data, DirectoryInfo folder, string fileName)
    {
        string json = JsonSerializer.Serialize(data, DATASET_JSON_OPTIONS);
        string filePath = Path.Combine(folder.FullName, fileName + ".json");
        File.WriteAllText(filePath, json, DATASET_ENCODING);
    }
}
