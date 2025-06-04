namespace Domain.Interfaces;

public interface IScore
{
    string Name { get; set; }
    Dictionary<string, int> Votes { get; set; }
}
