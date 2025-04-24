namespace Bitredict.Dtos.Response;

public class GetBetOfTheDayBankersAndSlipOfTheDay
{
    public int Id { get; set; }
    public string Teams { get; set; }
    public string? MatchDate { get; set; }
    public string Odds1 { get; set; }
    public string OddsX { get; set; }
    public string Odds2 { get; set; }
    public string Tip { get; set; }
    public string Goals { get; set; }
    public string Gg { get; set; }
    public string BestTip { get; set; }
    public string Trust { get; set; }
}
