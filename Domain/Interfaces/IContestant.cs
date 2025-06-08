namespace Domain.Interfaces;

public interface IContestant
{
    int Id { get; set; }
    string Country { get; set; }
    string Artist { get; set; }
    IEnumerable<string> ArtistPeople { get; set; }
    string Song { get; set; }
    string[] VideoUrls { get; set; }
    string[] Dancers { get; set; }
    string[] Backings { get; set; }
    string[] Composers { get; set; }
    string[] Lyricists { get; set; }
    string[] Writers { get; set; }
    string Conductor { get; set; }
    string StageDirector { get; set; }
    string Tone { get; set; }
    int? Bpm { get; set; }
    string Broadcaster { get; set; }
    string Spokesperson { get; set; }
    string[] Commentators { get; set; }
}
