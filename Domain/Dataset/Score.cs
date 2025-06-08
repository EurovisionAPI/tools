using Domain.Interfaces;

namespace Domain.Dataset;

public class Score : IScore
{
    public string Name { get; set; }
    public Dictionary<string, int> Votes { get; set; }
}
