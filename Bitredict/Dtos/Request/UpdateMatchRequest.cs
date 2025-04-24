namespace Bitredict.Dtos.Request;

public class UpdateMatchRequest
{
    public int Id { get; set; }
   // public DateOnly CreatedDate { get; set; }
    public string? League { get; set; } = null;
    public string? BetOfTheDayStatus { get; set; }
    public string? MatchStatus { get; set; }
    public string Teams { get; set; }
    public string? MatchDate { get; set; }
    public string? Odds1 { get; set; }
    public string? OddsX { get; set; }
    public string? Odds2 { get; set; }
    public string? Tip { get; set; }
    public string? TipOdd { get; set; }= null;
    public string? Goals { get; set; }
    public string? GoalsOdd { get; set; } = null;
    public string? Gg { get; set; }
    public string? GgOdd { get; set; } = null;
    public string? BestTip { get; set; }
    public string? BestTipOdd { get; set; } = null;
    public string? Trust { get; set; }
}
