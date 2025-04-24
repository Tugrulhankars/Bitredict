namespace Bitredict.Dtos.Request;

public class AddMatchRequest
{
    public string? Teams { get; set; } = null;
    public string? MatchDate { get; set; } = null;
    public string? MatchStatus { get; set; }
    public string? Odds1 { get; set; } = null;
    public string? OddsX { get; set; } = null;
    public string? Odds2 { get; set; } = null;
    public string? Tip { get; set; } = null;
    public string? Goals { get; set; } = null;
    public string? Gg { get; set; } = null;
    public string? BestTip { get; set; } = null;
    public string? Trust { get; set; } = null;
}
