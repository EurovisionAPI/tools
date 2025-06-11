namespace Domain.Interfaces;

public interface IContestant
{
    #region General Information

    int Id { get; set; }
    string Country { get; set; }
    string Artist { get; set; }
    string Song { get; set; }
    string[] VideoUrls { get; set; }
    int? Bpm { get; set; }
    string Tone { get; set; } 

    #endregion

    #region On-Stage Representation

    string[] ArtistPeople { get; set; }
    string[] Backings { get; set; }
    string[] Dancers { get; set; }
    string StageDirector { get; set; }

    #endregion

    #region Musical Team

    string[] Composers { get; set; }
    string Conductor { get; set; }
    string[] Lyricists { get; set; }
    string[] Writers { get; set; }

    #endregion

    #region Broadcasting

    string Broadcaster { get; set; }
    string[] Commentators { get; set; }
    string[] Jury { get; set; }
    string Spokesperson { get; set; }     

    #endregion
}
