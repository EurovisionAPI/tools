namespace Scanner;

internal class ContestData
{
    public UnavailableData UnavailableContestData { get; set; }
    public IReadOnlyCollection<UnavailableData> UnavailableContestantsData { get; set; }
    public bool HasUnavailableData =>
        (UnavailableContestData?.HasUnavailableData ?? false)
        ||
        (UnavailableContestantsData?.Any(data => data.HasUnavailableData) ?? false);
}
