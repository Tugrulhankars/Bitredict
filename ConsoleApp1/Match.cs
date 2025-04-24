using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1;

public class Match : BaseEntity
{
    public int Id { get; set; }
    public string? League { get; set; }
    public string MatchStatus { get; set; }
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

    public Match()
    {
        CreatedDate = DateOnly.FromDateTime(DateTime.Now);
    }
}
