using System.Text.Json;
using Domain.Scraper;
using Domain.Shared;

namespace PointsFixer;

internal class Program
{
    private static readonly JsonSerializerOptions JSON_OPTIONS = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    static void Main(string[] args)
    {
        FixContests("senior.json");
        //FixContests("junior.json");
    }

    private static void FixContests(string filename)
    {
        Contest[] noPoints = ReadJson<Contest[]>("no", filename);
        Contest[] points = ReadJson<Contest[]>("si", filename);

        foreach (Contest no in noPoints) 
        {
            Contest yes = points.First(c => c.Year == no.Year);

            foreach (Round noRound in no.Rounds)
            {
                Round yesRound = yes.Rounds.First(r => r.Name == noRound.Name);

                if (noRound.Performances != null)
                {
                    foreach (Performance noPerf in noRound.Performances)
                    {
                        Performance yesPerf = yesRound.Performances.First(p => p.ContestantId == noPerf.ContestantId);

                        foreach (Score noScore in noPerf.Scores)
                        {
                            Score yesScore = yesPerf.Scores.First(s => noScore.Name.Contains(s.Name));

                            noScore.Points = yesScore.Points;
                        }
                    }
                }
            }
        }

        string json = JsonSerializer.Serialize(noPoints, JSON_OPTIONS);
        File.WriteAllText(filename, json);
    }

    private static T ReadJson<T>(string folder, string filename)
    {
        string json = File.ReadAllText(Path.Combine(folder, filename));
        return JsonSerializer.Deserialize<T>(json, JSON_OPTIONS);
    }
}
