using HtmlAgilityPack;
using System.Net;
using System;
using System.Text.RegularExpressions;
using ConsoleApp1;
using Match = ConsoleApp1.Match;
using System.Reflection.Metadata;


using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;




//public class Program
//{
//    public static void Main()
//    {
//        var url = "https://nerdytips.com/all-matches"; // HTML kaynağınızın olduğu URL
//        HtmlWeb web = new HtmlWeb();
//        HtmlDocument doc = web.Load(url);

//        List<MatchInfo> matches = new List<MatchInfo>();

//        var leagueNode = doc.DocumentNode.SelectSingleNode("//button[@type='button']");
//        string leagueName = leagueNode?.InnerText.Trim();

//        var rows = doc.DocumentNode.SelectNodes("//tr[contains(@class, 'matchxd')]");
//        foreach (var row in rows)
//        {
//            MatchInfo match = new MatchInfo
//            {
//                League = leagueName,
//                Time = row.SelectSingleNode(".//td[contains(@class, 'td-data')]").InnerText.Trim(),
//                HomeTeam = row.SelectSingleNode(".//div[contains(@class, 'score-left')]/span").InnerText.Trim(),
//                AwayTeam = row.SelectSingleNode(".//div[contains(@class, 'score-right')]/span").InnerText.Trim(),
//                HomeWinOdds = decimal.Parse(row.SelectSingleNode(".//td[contains(@class, 'td-1')]//div[@class='progress-bar']").InnerText.Trim()),
//                DrawOdds = decimal.Parse(row.SelectSingleNode(".//td[contains(@class, 'td-x')]//div[@class='progress-bar']").InnerText.Trim()),
//                AwayWinOdds = decimal.Parse(row.SelectSingleNode(".//td[contains(@class, 'td-2')]//div[@class='progress-bar']").InnerText.Trim()),
//                Tip = row.SelectSingleNode(".//td[contains(@class, 'td-tip')]//p").InnerText.Trim(),
//                Goals = row.SelectSingleNode(".//td[contains(@class, 'td-goals')]//p").InnerText.Trim(),
//                BothTeamsToScore = row.SelectSingleNode(".//td[contains(@class, 'td-btts')]//p").InnerText.Trim(),
//                BestTip = row.SelectSingleNode(".//td[contains(@class, 'td-best-tips')]//p").InnerText.Trim(),
//                Trust = row.SelectSingleNode(".//td[contains(@class, 'td-trust')]").InnerText.Trim()
//            };

//            matches.Add(match);
//        }

//        foreach (var match in matches)
//        {
//            Console.WriteLine($"League: {match.League}");
//            Console.WriteLine($"Time: {match.Time}");
//            Console.WriteLine($"Match: {match.HomeTeam} vs {match.AwayTeam}");
//            Console.WriteLine($"Odds - Home Win: {match.HomeWinOdds}, Draw: {match.DrawOdds}, Away Win: {match.AwayWinOdds}");
//            Console.WriteLine($"Tip: {match.Tip}");
//            Console.WriteLine($"Goals: {match.Goals}");
//            Console.WriteLine($"Both Teams To Score: {match.BothTeamsToScore}");
//            Console.WriteLine($"Best Tip: {match.BestTip}");
//            Console.WriteLine($"Trust: {match.Trust}");
//            Console.WriteLine();
//        }
//    }
//}
public class MatchInfo
{
    public string League { get; set; }
    public string Time { get; set; }
    public string HomeTeam { get; set; }
    public string AwayTeam { get; set; }
    public string HomeWinOdds { get; set; }
    public string DrawOdds { get; set; }
    public string AwayWinOdds { get; set; }
    public string Tip { get; set; }
    public string TipOdds { get; set; } // Tip oranı için yeni özellik
    public string Goals { get; set; }
    public string GoalsOdds { get; set; } // Goals oranı için yeni özellik
    public string BothTeamsToScore { get; set; }
    public string BothTeamsToScoreOdds { get; set; } // gg oranı için yeni özellik
    public string BestTip { get; set; }
    public string BestTipOdds { get; set; }
    public string Trust { get; set; }
}

public class Program
{
    public static async Task Main()
    {
        var url = "https://nerdytips.com/all-matches";
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Cookie", $"timezone=Europe/Istanbul; timezone_offset=120; country=Turkey; country_code=TR; continent=Asia; notification_read=true; PHPSESSID=e8f974cc15978b9e0c64005aea622da5; joinVisited=true; givenDate=2024-10-12 00:00:00; startDate=2024-10-11 22:00:00; endDate=2024-10-12 22:00:00; odds_format=EU");

        var html = await httpClient.GetStringAsync(url);


        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        List<MatchInfo> matches = new List<MatchInfo>();

        // Tüm ligleri ve maçları içeren `nt-lists` altında `nt-matches-list-item` divlerini seç
        var matchListItems = htmlDocument.DocumentNode.SelectNodes("//div[@id='accordion' and contains(@class, 'nt-lists')]//div[contains(@class, 'nt-matches-list-item')]");

        if (matchListItems != null)
        {
            foreach (var item in matchListItems)
            {
                try
                {
                    // Lig adını çek
                    var leagueNode = item.SelectSingleNode(".//button[@type='button']");
                    string leagueName = leagueNode?.InnerText.Trim();

                    // Maç bilgilerini seç
                    var rows = item.SelectNodes(".//tr[contains(@class, 'matchxd')]");
                    if (rows != null)
                    {
                        foreach (var row in rows)
                        {
                            // Odds verilerini string olarak alıp formatlama yapıyoruz
                            string homeWinRaw = row.SelectSingleNode(".//td[contains(@class, 'td-1')]//div[@class='progress-bar']").InnerText.Trim();
                            string drawRaw = row.SelectSingleNode(".//td[contains(@class, 'td-x')]//div[@class='progress-bar']").InnerText.Trim();
                            string awayWinRaw = row.SelectSingleNode(".//td[contains(@class, 'td-2')]//div[@class='progress-bar']").InnerText.Trim();

                            // Tip, Goals ve gg oranlarını alma
                            var tipNode = row.SelectSingleNode(".//td[contains(@class,'td-tip')]//p");
                            var tipOddsNode = row.SelectSingleNode(".//td[contains(@class,'td-tip')]//span");

                            var goalsNode = row.SelectSingleNode(".//td[contains(@class,'td-goals')]//p");
                            var goalsOddsNode = row.SelectSingleNode(".//td[contains(@class,'td-goals')]//span");

                            var ggNode = row.SelectSingleNode(".//td[contains(@class,'td-btts')]//p");
                            var ggOddsNode = row.SelectSingleNode(".//td[contains(@class,'td-btts')]//span");

                            string tip = tipNode?.InnerText.Trim() ?? "Tip bilgisi yok";
                            string tipOdds = tipOddsNode?.InnerText.Trim() ?? "Oran bilgisi yok";

                            string goals = goalsNode?.InnerText.Trim() ?? "Goals bilgisi yok";
                            string goalsOdds = goalsOddsNode?.InnerText.Trim() ?? "Oran bilgisi yok";

                            string bothTeamsToScore = ggNode?.InnerText.Trim() ?? "GG bilgisi yok";
                            string bothTeamsToScoreOdds = ggOddsNode?.InnerText.Trim() ?? "Oran bilgisi yok";

                            // BestTip ve Oranı alma
                            var bestTipNode = row.SelectSingleNode(".//td[contains(@class,'td-best-tips')]//p");
                            var bestTipOddsNode = row.SelectSingleNode(".//td[contains(@class,'td-best-tips')]//span");

                            string bestTip = bestTipNode?.InnerText.Trim() ?? "Tip bilgisi yok";
                            string bestTipOdds = bestTipOddsNode?.InnerText.Trim() ?? "Oran bilgisi yok";

                            MatchInfo match = new MatchInfo
                            {
                                League = leagueName,
                                Time = row.SelectSingleNode(".//td[contains(@class, 'td-data')]").InnerText.Trim(),
                                HomeTeam = row.SelectSingleNode(".//div[contains(@class, 'score-left')]/span").InnerText.Trim(),
                                AwayTeam = row.SelectSingleNode(".//div[contains(@class, 'score-right')]/span").InnerText.Trim(),
                                HomeWinOdds = homeWinRaw,
                                DrawOdds = drawRaw,
                                AwayWinOdds = awayWinRaw,
                                Tip = tip,
                                TipOdds = tipOdds, 
                                Goals = goals,
                                GoalsOdds = goalsOdds, 
                                BothTeamsToScore = bothTeamsToScore,
                                BothTeamsToScoreOdds = bothTeamsToScoreOdds, 
                                BestTip = bestTip,
                                BestTipOdds = bestTipOdds,
                                Trust = row.SelectSingleNode(".//td[contains(@class,'td-trust')]").InnerText.Trim()
                            };

                            matches.Add(match);
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine("Error processing match item data: " + ex.Message);
                }
            }
        }
        else
        {
            Console.WriteLine("No match list items found under nt-lists.");
        }

        // Maç bilgilerini ekrana yazdır
        foreach (var match in matches)
        {
            Console.WriteLine($"League: {match.League}");
            Console.WriteLine($"Time: {match.Time}");
            Console.WriteLine($"Match: {match.HomeTeam} vs {match.AwayTeam}");
            Console.WriteLine($"Odds - Home Win: {match.HomeWinOdds}, Draw: {match.DrawOdds}, Away Win: {match.AwayWinOdds}");
            Console.WriteLine($"Tip: {match.Tip}, Odds: {match.TipOdds}"); // Oranı ile birlikte yazdırıyoruz
            Console.WriteLine($"Goals: {match.Goals}, Odds: {match.GoalsOdds}"); // Oranı ile birlikte yazdırıyoruz
            Console.WriteLine($"Both Teams To Score: {match.BothTeamsToScore}, Odds: {match.BothTeamsToScoreOdds}"); // Oranı ile birlikte yazdırıyoruz
            Console.WriteLine($"Best Tip: {match.BestTip}, Odds: {match.BestTipOdds}"); // Oranı ile birlikte yazdırıyoruz
            Console.WriteLine($"Trust: {match.Trust}");
            Console.WriteLine("---------");
        }
    }
}
// Odds değ
//var url = "https://nerdytips.com/all-matches";
//var httpClient = new HttpClient();
//var html = await httpClient.GetStringAsync(url);

//var htmlDocument = new HtmlDocument();
//htmlDocument.LoadHtml(html);

//// nt-lists altında bulunan nt-matches-list-item divlerini seç
//var matchListItems = htmlDocument.DocumentNode.SelectNodes("//div[@id='accordion' and contains(@class, 'nt-lists')]//div[contains(@class, 'nt-matches-list-item')]");

//if (matchListItems != null)
//{
//    foreach (var item in matchListItems)
//    {
//        try
//        {
//            var content = item.InnerText.Trim();
//            Console.WriteLine("Match Item Content: " + content);
//            Console.WriteLine("---------");
//        }
//        catch (NullReferenceException ex)
//        {
//            Console.WriteLine("Error processing match item data: " + ex.Message);
//        }
//    }
//}
//else
//{
//    Console.WriteLine("No match list items found under nt-lists.");
//}



//var matchRows = doc.DocumentNode.SelectNodes("//tbody/tr");

//if (matchRows != null)
//{
//    foreach (var row in matchRows)
//    {
//        var timeNode = row.SelectSingleNode(".//p[contains(@class, 'score-time')]");
//        var time = timeNode?.InnerText.Trim();

//        var teamsNode = row.SelectSingleNode(".//td[contains(@class, 'td-match')]");
//        var teams = teamsNode?.InnerText.Trim();

//        var leagueNode = row.SelectSingleNode(".//button[@class='btn btn-link']");
//        var league = leagueNode?.InnerText.Trim();

//        var odds1Node = row.SelectSingleNode(".//td[contains(@class, 'td-1')]");
//        var odds1 = odds1Node?.InnerText.Trim();

//        var oddsXNode = row.SelectSingleNode(".//td[contains(@class, 'td-x')]");
//        var oddsX = oddsXNode?.InnerText.Trim();

//        var odds2Node = row.SelectSingleNode(".//td[contains(@class, 'td-2')]");
//        var odds2 = odds2Node?.InnerText.Trim();

//        var tipsNode = row.SelectSingleNode(".//td[contains(@class,'td-tip')]");
//        var tips = tipsNode?.InnerText.Trim();

//        var goalsNode = row.SelectSingleNode(".//td[contains(@class,'td-goals')]");
//        var goals = goalsNode?.InnerText.Trim();

//        var ggNode = row.SelectSingleNode(".//td[contains(@class,'td-btts')]");
//        var gg = ggNode?.InnerText.Trim();

//        var bestTipNode = row.SelectSingleNode(".//td[contains(@class,'td-best-tip')]");
//        var bestTip = bestTipNode?.InnerText.Trim();

//        var trustNode = row.SelectSingleNode(".//td[contains(@class,'td-trust')]");
//        var trust = trustNode?.InnerText.Trim();

//        // Print the extracted values to the console
//        Console.WriteLine($"Time: {time}");
//        Console.WriteLine($"Teams: {teams}");
//        Console.WriteLine($"League: {league}");
//        Console.WriteLine($"Odds 1: {odds1}");
//        Console.WriteLine($"Odds X: {oddsX}");
//        Console.WriteLine($"Odds 2: {odds2}");
//        Console.WriteLine($"Tips: {tips}");
//        Console.WriteLine($"Goals: {goals}");
//        Console.WriteLine($"GG: {gg}");
//        Console.WriteLine($"Best Tip: {bestTip}");
//        Console.WriteLine($"Trust: {trust}");
//        Console.WriteLine("-----------------------------------------");
//    }
//}

//var buttonNode = doc.DocumentNode.SelectSingleNode("//button[@class='btn btn-link']");

//if (buttonNode != null)
//{
//    // Button içindeki metni alın
//    var buttonText = buttonNode.InnerText.Trim();
//    Console.WriteLine("Button text: " + buttonText);
//}
//else
//{
//    Console.WriteLine("Button bulunamadı.");
//}





//var predictedTodayValueNode = doc.DocumentNode.SelectSingleNode("//div[@class='single-statics no-border']//span[@class='counter']");
//var predictedTodayTextNode = doc.DocumentNode.SelectSingleNode("//div[@class='single-statics no-border']//p");

//if (predictedTodayValueNode != null && predictedTodayTextNode != null)
//{
//    var predictedTodayValue = predictedTodayValueNode.InnerText.Trim();
//    var predictedTodayText = predictedTodayTextNode.InnerText.Trim();
//    Console.WriteLine($"{predictedTodayText}: {predictedTodayValue}");
//}
//else
//{
//    Console.WriteLine("Predicted Today verisi bulunamadı.");
//}

//// Bankers Rate verisini al
//var bankersRateValueNode = doc.DocumentNode.SelectSingleNode("(//div[@class='text-box']//span[@class='counter'])[2]");
//var bankersRateTextNode = doc.DocumentNode.SelectSingleNode("(//div[@class='text-box']//p)[2]");

//if (bankersRateValueNode != null && bankersRateTextNode != null)
//{
//    var bankersRateValue = bankersRateValueNode.InnerText.Trim();
//    var bankersRateText = bankersRateTextNode.InnerText.Trim();
//    Console.WriteLine($"{bankersRateText}: {bankersRateValue}");
//}
//else
//{
//    Console.WriteLine("Bankers Rate verisi bulunamadı.");
//}

//// Matches Won verisini al
//var matchesWonValueNode = doc.DocumentNode.SelectSingleNode("(//div[@class='text-box']//span[@class='counter'])[3]");
//var matchesWonTextNode = doc.DocumentNode.SelectSingleNode("(//div[@class='text-box']//p)[3]");

//if (matchesWonValueNode != null && matchesWonTextNode != null)
//{
//    var matchesWonValue = matchesWonValueNode.InnerText.Trim();
//    var matchesWonText = matchesWonTextNode.InnerText.Trim();
//    Console.WriteLine($"{matchesWonText}: {matchesWonValue}");
//}
//else
//{
//    Console.WriteLine("Matches Won verisi bulunamadı.");
//}




//var predictedNode = doc.DocumentNode.SelectSingleNode("//span[@id='predicted_count']");
//        string predictedCount = predictedNode != null ? predictedNode.InnerText.Trim() : "Bulunamadı";

//        // Upcoming değeri
//        var upcomingNode = doc.DocumentNode.SelectSingleNode("//span[@id='notstarted_count']");
//        string upcomingCount = upcomingNode != null ? upcomingNode.InnerText.Trim() : "Bulunamadı";

//        // Won Matches değeri
//        var wonMatchesNode = doc.DocumentNode.SelectSingleNode("//span[@id='success_rate']");
//        string wonMatchesCount = wonMatchesNode != null ? wonMatchesNode.InnerText.Trim() : "Bulunamadı";

//        // Değerleri yazdırmak
//        Console.WriteLine("Predicted: " + predictedCount);   // Beklenen çıktı: 34
//        Console.WriteLine("Upcoming: " + upcomingCount);     // Beklenen çıktı: 8
//        Console.WriteLine("Won Matches: " + wonMatchesCount); // Beklenen çıktı: 20

//        // Hata durumunu kontrol etmek için ek bilgi
//        if (predictedNode == null)
//            Console.WriteLine("Predicted verisi bulunamadı, HTML yapısını tekrar kontrol edin.");

//        if (upcomingNode == null)
//            Console.WriteLine("Upcoming verisi bulunamadı, HTML yapısını tekrar kontrol edin.");

//        if (wonMatchesNode == null)
//            Console.WriteLine("Won Matches verisi bulunamadı, HTML yapısını tekrar kontrol edin.");
