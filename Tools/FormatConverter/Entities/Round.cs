namespace FormatConverter.Entities;

public class Round
{
    public string Name { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? Time { get; set; }
    public int[] Disqualifieds { get; set; }
    public Performance[] Performances { get; set; }
}
