namespace Scanner;

internal class ContestScanResult : UnavailableData
{
    private List<UnavailableData> _unavailableContestants;

    public IReadOnlyList<UnavailableData> Contestants => _unavailableContestants;
    public override bool HasUnavailableData => base.HasUnavailableData
        || _unavailableContestants.Any(data => data.HasUnavailableData);

    public ContestScanResult(int year) : base(year.ToString())
    {
        _unavailableContestants = new List<UnavailableData>();
    }

    public void AddUnavailableContestant(UnavailableData unavailableData)
    {
        _unavailableContestants.Add(unavailableData);
    }
}
