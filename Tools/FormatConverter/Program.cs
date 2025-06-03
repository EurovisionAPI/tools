using System.Globalization;

namespace FormatConverter;

internal class Program
{
    static void Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        Properties.ReadArguments(args);

        Transformer transformer = new Transformer();
        //transformer.ToDatasetFormat("eurovision.json", "senior");
        transformer.ToDatasetFormat("junior.json", "junior");
    }
}
/*
private static void ChangeFormat(string fileName, string folderName)
{
    Contest[] contests = ReadContests(fileName);
    DirectoryInfo folder = Directory.CreateDirectory(folderName);

    foreach (Contest contest in contests)
    {
        StoreContest(folder, contest);
    }
}

private static Contest[] ReadContests(string fileName)
{
    JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    string json = File.ReadAllText(fileName);
    Contest[] contests = JsonSerializer.Deserialize<Contest[]>(json, serializerOptions);

    return contests;
}

private static void StoreContest(DirectoryInfo parentFolder, Contest contest)
{
    DirectoryInfo folder = parentFolder.CreateSubdirectory(contest.Year.ToString());
    ContestStore contestStore = new ContestStore(contest);

    Store(contestStore, folder, "contest");
    StoreContestants(folder, contest.Contestants);
    StoreRounds(folder, contest.Rounds);
}

private static void StoreContestants(DirectoryInfo parentFolder, Contestant[] contestants)
{
    DirectoryInfo folder = parentFolder.CreateSubdirectory("contestants");

    foreach (Contestant contestant in contestants)
    {
        StoreContestant(folder, contestant);
    }
}

private static void StoreContestant(DirectoryInfo parentFolder, Contestant contestant)
{
    string folderName = $"{contestant.Id}_{contestant.Country.ToLower()}";
    DirectoryInfo folder = parentFolder.CreateSubdirectory(folderName);
    ContestantStore contestantStore = new ContestantStore(contestant);

    Store(contestantStore, folder, "contestant");
    StoreLyrics(folder, contestant.Lyrics);
}

private static void StoreLyrics(DirectoryInfo parentFolder, Lyrics[] allLyrics)
{
    DirectoryInfo folder = parentFolder.CreateSubdirectory("lyrics");

    foreach (Lyrics lyrics in allLyrics)
    {
        StoreLyrics(folder, lyrics);
    }
}

private static void StoreLyrics(DirectoryInfo folder, Lyrics lyrics)
{
    string type = lyrics.IsVersion
        ? "v"   // Version
        : "t";  // Translation

    string languages = string.Join(",", lyrics.Languages);
    string displayedLanguages = string.Join(",", lyrics.DisplayedLanguages);

    string fileName = $"{string.Join("_", type, languages, displayedLanguages)}.txt";
    string fileContent = $"{lyrics.Title}\n\n{lyrics.Content}";
    File.WriteAllText(Path.Combine(folder.FullName, fileName), fileContent);
}

private static void StoreRounds(DirectoryInfo parentFolder, Round[] rounds)
{
    DirectoryInfo folder = parentFolder.CreateSubdirectory("rounds");

    foreach (Round round in rounds)
    {
        StoreRound(folder, round);
    }
}

private static void StoreRound(DirectoryInfo folder, Round round)
{
    RoundStore roundStore = new RoundStore(round);
    Store(roundStore, folder, round.Name);
}

private static void Store<T>(T data, DirectoryInfo folder, string fileName)
{
    JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,//JavaScriptEncoder.Create(UnicodeRanges.All),
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase // si quieres comportamiento similar a .Web
    };

    string json = JsonSerializer.Serialize(data, serializerOptions);
    Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    File.WriteAllText(Path.Combine(folder.FullName, fileName + ".json"), json, encoding);
}
}

class ContestStore2
{
public int Year { get; set; }
public string Arena { get; set; }
public string City { get; set; }
public string Country { get; set; }
public string IntendedCountry { get; set; }
public string Slogan { get; set; }
public string[] Presenters { get; set; }
public string[] Broadcasters { get; set; }
public IEnumerable<ContestantStore> Contestants { get; set; }
public IEnumerable<RoundStore> Rounds { get; set; }

public ContestStore2(Contest contest)
{
    Year = contest.Year;
    Arena = contest.Arena;
    City = contest.City;
    Country = contest.Country;
    IntendedCountry = contest.IntendedCountry;
    Slogan = contest.Slogan;
    Presenters = contest.Presenters;
    Broadcasters = contest.Broadcasters;
    Contestants = contest.Contestants.Select(c => new ContestantStore(c));
    Rounds = contest.Rounds.Select(r => new RoundStore(r));
}
}

class ContestStore
{
public int Year { get; set; }
public string Arena { get; set; }
public string City { get; set; }
public string Country { get; set; }
public string IntendedCountry { get; set; }
public string Slogan { get; set; }
public string[] Presenters { get; set; }
public string[] Broadcasters { get; set; }

public ContestStore(Contest contest)
{
    Year = contest.Year;
    Arena = contest.Arena;
    City = contest.City;
    Country = contest.Country;
    IntendedCountry = contest.IntendedCountry;
    Slogan = contest.Slogan;
    Presenters = contest.Presenters;
    Broadcasters = contest.Broadcasters;
}
}

class ContestantStore
{
public int Id { get; set; }
public string Country { get; set; }
public string Broadcaster { get; set; }
public string Artist { get; set; }
public string Song { get; set; }
public string[] VideoUrls { get; set; }
public int? Bpm { get; set; }
public string Tone { get; set; }
public string[] Backings { get; set; }
public string[] Commentators { get; set; }
public string[] Composers { get; set; }
public string Conductor { get; set; }
public string[] Dancers { get; set; }
public string[] Lyricists { get; set; }
public string Spokesperson { get; set; }
public string StageDirector { get; set; }
public string[] Writers { get; set; }

public ContestantStore(Contestant contestant)
{
    Id = contestant.Id;
    Country = contestant.Country;
    Broadcaster = contestant.Broadcaster;
    Artist = contestant.Artist;
    Song = contestant.Song;
    VideoUrls = contestant.VideoUrls;
    Bpm = contestant.Bpm;
    Tone = contestant.Tone;
    Backings = contestant.Backings;
    Commentators = contestant.Commentators;
    Composers = contestant.Composers;
    Conductor = contestant.Conductor;
    Dancers = contestant.Dancers;
    Lyricists = contestant.Lyricists;
    Spokesperson = contestant.Spokesperson;
    StageDirector = contestant.StageDirector;
    Writers = contestant.Writers;
}
}

class RoundStore
{
public DateOnly Date { get; set; }
public TimeOnly? Time { get; set; }
public int[] Disqualifieds { get; set; }
public IEnumerable<PerformanceStore>? Performances { get; set; }

public RoundStore(Round round)
{
    Date = round.Date;
    Time = round.Time;
    Disqualifieds = round.Disqualifieds;
    Performances = round.Performances?.Select(p => new PerformanceStore(p));
}
}

class PerformanceStore
{
public int ContestantId { get; set; }
public int Running { get; set; }
public int Place { get; set; }
public IEnumerable<ScoreStore> Scores { get; set; }

public PerformanceStore(Performance performance)
{
    ContestantId = performance.ContestantId;
    Running = performance.Running;
    Place = performance.Place;
    Scores = performance.Scores.Select(s => new ScoreStore(s));
}
}

class ScoreStore
{
public string Name { get; set; }
public Dictionary<string, int> Votes { get; set; }

public ScoreStore(Score score)
{
    Name = score.Name;
    Votes = score.Votes;
}
}

*/