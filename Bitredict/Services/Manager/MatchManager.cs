using AutoMapper;
using Bitredict.DataAccess.Abstract;
using Bitredict.Dtos.Request;
using Bitredict.Dtos.Response;
using Bitredict.Models;
using Bitredict.Services.Abstracts;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Match = Bitredict.Models.Match;

namespace Bitredict.Services.Manager;

public class MatchManager : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    public MatchManager(IMatchRepository matchRepository, IMapper mapper, IConfiguration configuration)
    {
        _matchRepository = matchRepository;
        _mapper = mapper;
        _configuration = configuration;
    }



    //custom maç eklemek için 
    public async Task<AddMatchResponse> AddMatch(AddMatchRequest addMatchRequest)
    {

        var match = _mapper.Map<Match>(addMatchRequest);
        var response = _mapper.Map<AddMatchResponse>(addMatchRequest);
        await _matchRepository.Add(match);
        return response;
    }


    // https://nerdytips.com/bet-of-the-day adrese gider maçları getiri önce veritabanına sonra kayıt eder 
    public async Task AddBetOfTheDayMatchDatabase()
    {
        string url = "https://nerdytips.com/bet-of-the-day";
        await GetBetOfTheDayMathcAndSaveDatabase(url, "Bet Of The Day");
      //  await GetMatchInWebSiteAndSaveDatabase(url, "Bet Of The Day");

    }

    public async Task AddAllMacthDatabase()
    {
        string url = "https://nerdytips.com/all-matches";
       // await GetMatchInWebSiteAndSaveDatabase(url,"all matches");
        await GetMatchInWebSiteAndSaveDatabase(url, "All Match"); 
    }

    //veritaba'nından maçları getirir
    public async Task<List<GetAllMatchResponse>> GetAllMatchInDatabase()
    {
        //var result = await _matchRepository.GetAll();
        var result = await _matchRepository.GetAll();
        List<GetAllMatchResponse> response = _mapper.Map<List<GetAllMatchResponse>>(result);
        return response;
    }
    public async Task<List<GetBetOfTheDatMatchReponse>> GetBetOfTheDayMatch()
    {
        List<Match> result = await _matchRepository.GetAll();
        List<GetBetOfTheDatMatchReponse> betOf = new();
        foreach (var item in result)
        {
            if (item.MatchStatus == "Bet Of The Day")
            {
                GetBetOfTheDatMatchReponse response = _mapper.Map<GetBetOfTheDatMatchReponse>(item);
                betOf.Add(response);
            }
        }
        return betOf;
    }
   
    //custom maç güncellemek için
    public async Task<UpdateMatchResponse> UpdateMatch(UpdateMatchRequest updateMatchRequest)
    {
        Match createdDate = await _matchRepository.GetMatchById(updateMatchRequest.Id);
        var updateMatch = _mapper.Map<Match>(updateMatchRequest);
        var updateResponse = _mapper.Map<UpdateMatchResponse>(updateMatch);
        if ( updateMatchRequest.Odds1=="string")
        {
            _matchRepository.UpdateMacthScore(updateMatchRequest.Id,updateMatchRequest.Teams,updateMatchRequest.MatchDate);
            return updateResponse;
        }
        else
        {
            updateMatch.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
            updateMatch.CreatedDate = createdDate.CreatedDate;
            await _matchRepository.Update(updateMatch);
            return updateResponse;


        }




        

    }

    //https://nerdytips.com/bet-of-the-day adresdeki maçları getirir ve liste şeklinde geri döner 
    // not veritabanına kayıt yapmaz
    public async Task<List<GetAllMatchResponse>> GetDirectBetOfTheDayMatch()
    {
        string url = "https://nerdytips.com/bet-of-the-day";
        var response = await GetMatchInWebSite(url, "Bet of The Day");
        return response;
    }
    // https://nerdytips.com/all-matches adresine gider ve bize tüm maçları döner
    public async Task<List<GetAllMatchResponse>> GetAllMatchInWebSite()
    {
        string url = "https://nerdytips.com/all-matches";
        var result = await GetMatchInWebSite(url, "All Match");
        // List<GetAllMatchResponse> response = _mapper.Map<List<GetAllMatchResponse>>(result);
        return result;
    }
    //verdiğimiz id'ye sahip maçları siler
    public async Task DeleteMatch(DeleteMatchRequest deleteMatch)
    {
        var match = _mapper.Map<Match>(deleteMatch);
        await _matchRepository.Delete(match);
    }
    public async Task AddBetOfTheDay(List<AddBetOfTheDay> addBetOfTheDay)
    {
        List<AddBetOfTheDay> slipOfTheDay = new List<AddBetOfTheDay>(); 
        List<AddBetOfTheDay> bankers = new List<AddBetOfTheDay>();
        List<AddBetOfTheDay> slipAndBankers= new List<AddBetOfTheDay>();
        List<AddBetOfTheDay> allList = new List<AddBetOfTheDay>();
      
        foreach (var item in addBetOfTheDay)
        {
            if (item.BetOfTheDayStatus=="Slip Of The Day")
            {
               
                slipOfTheDay.Add(item);

            }
            else if (item.BetOfTheDayStatus == "Bankers")
            {
                bankers.Add(item);
            }else if (item.BetOfTheDayStatus== "Bankers And Slip Of The Day")
            {
                slipAndBankers.Add(item);
            }

           
            //foreach (var slipOf in slipOfTheDay)
            //{
            //    if (item.Id==slipOf.Id && slipList==false)
            //    {
            //        item.BetOfTheDayStatus = "Bankers And Slip Of The Day";

            //    }
            //}
       
        }
        allList.AddRange(slipOfTheDay);
        allList.AddRange(bankers);
        allList.AddRange(slipAndBankers);
          List<Match> matches = _mapper.Map<List<Match>>(allList);
        await _matchRepository.AddBetOfTheDayMatch(matches);
         
        //if (slipOfTheDay.Count==0 && bankers.Count==0)
        //{
        //    foreach (var item in slipOfTheDay)
        //    {
        //        foreach (var ban in bankers)
        //        {
        //            if (item.Id == ban.Id)
        //            {
        //                item.BetOfTheDayStatus = "Bankers And Slip Of The Day";
        //                slipAndBankers.Add(item);
        //            }
        //            if (item.Id != ban.Id)
        //            {
        //                slipAndBankers.Add(item);
        //                slipAndBankers.Add(ban);
        //            }
        //        }
        //    }
        //    List<Match> matches = _mapper.Map<List<Match>>(slipAndBankers);
        //    await _matchRepository.AddBetOfTheDayMatch(matches);
        //}else if(slipOfTheDay.Count!=0 || bankers.Count != 0)
        //{

        //    List<Match> matches = _mapper.Map<List<Match>>(slipOfTheDay);
        //    List<Match> matches2 = _mapper.Map<List<Match>>(bankers);
        //    await _matchRepository.AddBetOfTheDayMatch(matches2);
        //    await _matchRepository.AddBetOfTheDayMatch(matches);

        //}


        //throw new NotImplementedException();
    }

    public async Task<List<GetBetOfTheDayBakers>> GetBetOfTheDayBakersMatch(DateOnly dateOnly)
    {
        try
        {
            List<GetBetOfTheDayBakers> list = new List<GetBetOfTheDayBakers>();

            var result = await _matchRepository.GetBetOfTheDayBakersMatch(dateOnly);
            var result2 = await _matchRepository.GetBetOfTheDayBankersAndSlipOfTheDayMatch(dateOnly);

            List<GetBetOfTheDayBakers> bankers = _mapper.Map<List<GetBetOfTheDayBakers>>(result);
            List<GetBetOfTheDayBakers> slip2 = _mapper.Map<List<GetBetOfTheDayBakers>>(result2);
            list.AddRange(slip2);
            list.AddRange(bankers);

            return list.ToList(); 
        }
        catch (Exception e)
        {

            throw e;
        }
        
    }

    public async Task<List<GetBetOfTheDaySlipOfTheDay>> GetBetOfTheDaySlipOfTheDayMatch(DateOnly dateOnly)
    {
        List<GetBetOfTheDaySlipOfTheDay> list = new List<GetBetOfTheDaySlipOfTheDay>();
        var result =await _matchRepository.GetBetOfTheDaySlipOfTheDayMatch(dateOnly);
        var result2 = await _matchRepository.GetBetOfTheDayBankersAndSlipOfTheDayMatch(dateOnly);
        List<GetBetOfTheDaySlipOfTheDay> slip = _mapper.Map<List<GetBetOfTheDaySlipOfTheDay>>(result);
        List<GetBetOfTheDaySlipOfTheDay> slip2 = _mapper.Map<List<GetBetOfTheDaySlipOfTheDay>>(result2);
        list.AddRange(slip);
        list.AddRange(slip2);
        return list.ToList();
    }
    //parametreye verilen tarihteki maçları getirir
    public async Task<List<GetAllMatchResponse>> GetByDateMatch(DateOnly dateMatch)
    {



        try
        {
            var result = await _matchRepository.GetByDateMatch(dateMatch);
            List<GetAllMatchResponse> response = _mapper.Map<List<GetAllMatchResponse>>(result);
            return response;
        }
        catch (Exception e)
        {

            throw e;
        }


    }
    public async Task<List<GetBetOfTheDatMatchReponse>> GetBetOfTheDayMatchByDate(DateOnly dateMatch)
    {
        var result = await _matchRepository.GetBetOfTheDayMatchByDate(dateMatch);
        List<GetBetOfTheDatMatchReponse> reponse = _mapper.Map<List<GetBetOfTheDatMatchReponse>>(result);
        return reponse;
    }
    public async Task<List<GetBetOfTheDayBankersAndSlipOfTheDay>> GetBetOfTheDayBankersAndSlipOfTheDay(DateOnly dateOnly)
    {
        var result =await _matchRepository.GetBetOfTheDayBankersAndSlipOfTheDayMatch(dateOnly);
        List<GetBetOfTheDayBankersAndSlipOfTheDay> response = _mapper.Map<List<GetBetOfTheDayBankersAndSlipOfTheDay>>(result);
        return response.ToList();
    }
    public async Task SetNewCookieValue(string PHPSESSID)
    {
        var cookieSettings = _configuration.GetSection("CookieSettings");
        cookieSettings.Value = PHPSESSID;
    }

    //web sitesinden tüm maçları getirir veri tabanına kayıt yapmaz geriye Maçları liste şeklinde döner
    public async Task<List<GetAllMatchResponse>> GetMatchInWebSite(string url, string macthStatus)
    {
        var cookie = _configuration.GetSection("CookieSettings");
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Cookie", $"timezone=Europe/Istanbul; timezone_offset=120; country=Turkey; country_code=TR; continent=Asia; notification_read=true; PHPSESSID={cookie.Value}; joinVisited=true; givenDate=2024-10-12 00:00:00; startDate=2024-10-11 22:00:00; endDate=2024-10-12 22:00:00; odds_format=EU");

        var html = await httpClient.GetStringAsync(url);
        List<GetAllMatchResponse> matches = new List<GetAllMatchResponse>();


        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var matchRows = htmlDoc.DocumentNode.SelectNodes("//tbody/tr");

        if (matchRows != null)
        {
            foreach (var row in matchRows)
            {

                var timeNode = row.SelectSingleNode(".//p[contains(@class, 'score-time')]");
                var time = timeNode?.InnerText.Trim() ?? "Saat bilgisi yok";


                var teamsNode = row.SelectSingleNode(".//td[contains(@class, 'td-match')]");


                var odds1Node = row.SelectSingleNode(".//td[contains(@class, 'td-1')]");
                var oddsXNode = row.SelectSingleNode(".//td[contains(@class, 'td-x')]");
                var odds2Node = row.SelectSingleNode(".//td[contains(@class, 'td-2')]");
                var tips = row.SelectSingleNode(".//td[contains(@class,'td-tip')]");
                var goals = row.SelectSingleNode(".//td[contains(@class,'td-goals')]");
                var gg = row.SelectSingleNode(".//td[contains(@class,'td-btts')]");
                var bestTip = row.SelectSingleNode(".//td[contains(@class,'td-best-tip')]");
                var trust = row.SelectSingleNode(".//td[contains(@class,'td-trust')]");
                var tipOddsNode = row.SelectSingleNode(".//td[contains(@class,'td-tip')]//span");
                var goalsOddsNode = row.SelectSingleNode(".//td[contains(@class,'td-goals')]//span");
                var ggOddsNode = row.SelectSingleNode(".//td[contains(@class,'td-btts')]//span");
                string tipOdds = tipOddsNode?.InnerText.Trim() ?? "Oran bilgisi yok";
                string goalsOdds = goalsOddsNode?.InnerText.Trim() ?? "Oran bilgisi yok";
                string bothTeamsToScoreOdds = ggOddsNode?.InnerText.Trim() ?? "Oran bilgisi yok";
                var bestTipOddsNode = row.SelectSingleNode(".//td[contains(@class,'td-best-tips')]//span");
                string bestTipOdds = bestTipOddsNode?.InnerText.Trim() ?? "Oran bilgisi yok";


                var team = CleanString(teamsNode?.InnerText);
                var odds1 = CleanString(odds1Node?.InnerText);
                var oddsX = CleanString(oddsXNode?.InnerText);
                var odds2 = CleanString(odds2Node?.InnerText);
                var tip = CleanString(tips?.InnerText);
                var goal = CleanString(goals?.InnerText);
                var g = CleanString(gg?.InnerText);
                var bTip = CleanString(bestTip?.InnerText);
                var trs = CleanString(trust?.InnerText);

                Match match = new();
                match.MatchStatus = macthStatus;
                match.MatchDate = time;
                match.Teams = team;
                match.Odds1 = odds1;
                match.OddsX = oddsX;
                match.Odds2 = odds2;
                match.Tip = tip;
                match.Goals = goal;
                match.Gg = g;
                match.BestTip = bTip;
                match.Trust = trs;
                GetAllMatchResponse response = _mapper.Map<GetAllMatchResponse>(match);

                matches.Add(response);




            }

        }

        return matches;



    }
    public static string CleanString(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return Regex.Replace(input, @"\s+", " ").Trim();
    }

    public async Task GetBetOfTheDayMathcAndSaveDatabase(string url,string matchStatus)
    {
        var httpClient = new HttpClient();
        var cookie = _configuration.GetSection("CookieSettings");
        httpClient.DefaultRequestHeaders.Add("Cookie", $"timezone=Europe/Istanbul; timezone_offset=120; country=Turkey; country_code=TR; continent=Asia; notification_read=true; PHPSESSID={cookie.Value}; joinVisited=true; givenDate=2024-10-12 00:00:00; startDate=2024-10-11 22:00:00; endDate=2024-10-12 22:00:00; odds_format=EU");
        var html = await httpClient.GetStringAsync(url);

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);


        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var matchRows = htmlDoc.DocumentNode.SelectNodes("//tbody/tr");

        if (matchRows != null)
        {
            List<Match> response = await _matchRepository.GetAll();

            foreach (var row in matchRows)
            {

                var timeNode = row.SelectSingleNode(".//p[contains(@class, 'score-time')]");
                var time = timeNode?.InnerText.Trim();


                var teamsNode = row.SelectSingleNode(".//td[contains(@class, 'td-match')]");

                var league = row.SelectSingleNode(".//button[@class='btn btn-link']");
                var odds1Node = row.SelectSingleNode(".//td[contains(@class, 'td-1')]");
                var oddsXNode = row.SelectSingleNode(".//td[contains(@class, 'td-x')]");
                var odds2Node = row.SelectSingleNode(".//td[contains(@class, 'td-2')]");
                var tips = row.SelectSingleNode(".//td[contains(@class,'td-tip')]");
                var goals = row.SelectSingleNode(".//td[contains(@class,'td-goals')]");
                var gg = row.SelectSingleNode(".//td[contains(@class,'td-btts')]");
                var bestTip = row.SelectSingleNode(".//td[contains(@class,'td-best-tip')]");
                var trust = row.SelectSingleNode(".//td[contains(@class,'td-trust')]");

                var team = CleanString(teamsNode?.InnerText);
                var odds1 = CleanString(odds1Node?.InnerText);
                var oddsX = CleanString(oddsXNode?.InnerText);
                var odds2 = CleanString(odds2Node?.InnerText);
                var leag = CleanString(league?.InnerText);
                var tip = CleanString(tips?.InnerText);
                var goal = CleanString(goals?.InnerText);
                var g = CleanString(gg?.InnerText);
                var bTip = CleanString(bestTip?.InnerText);
                var trs = CleanString(trust?.InnerText);

                Match match = new();
                match.MatchStatus = matchStatus;
                match.MatchDate = time;
                match.Teams = team.ToString();
                match.Odds1 = odds1.ToString();
                match.OddsX = oddsX.ToString();
                match.Odds2 = odds2.ToString();
                match.Tip = tip.ToString();
                match.Goals = goal.ToString();
                match.Gg = g.ToString();
                match.BestTip = bTip.ToString();
                match.Trust = trs.ToString();
                match.CreatedDate = DateOnly.FromDateTime(DateTime.Now.AddHours(2));

                if (match.MatchDate == null)
                {
                    match.MatchDate = "Match continue";
                }
                if (response.Count > 0)
                {
                    bool matchExists = false; // Flag to check if the match exists

                    foreach (var ite in response)
                    {
                        // Takımların eşleştiği durumu kontrol et
                        if (string.Equals(ite.Teams, match.Teams))
                        {
                            matchExists = true;

                            // Oranlar ve tipler eşleşiyorsa, maçın güncellenmesi
                            if (ite.Odds1 == match.Odds1 && ite.OddsX == match.OddsX && ite.Odds2 == match.Odds2)
                            {
                                // Tip farklı ise, güncellenebilir
                                if ((ite.Tip == "? ?" || ite.Tip == "?") && (match.Tip != "? ?" && match.Tip != "?"))
                                {
                                    // Burada sadece tip değiştirilmişse güncelleniyor
                                    await _matchRepository.Update(ite);
                                }
                                else
                                {
                                    // Oranlar eşleşiyor, ama tiplerde fark varsa başka işlem yapılabilir
                                    // Eğer başka bir işlem gerekiyorsa, buraya ekleyebilirsiniz.
                                }
                            }
                            else
                            {
                                // Oranlar farklıysa, bu durumda oranların değiştiği kabul edilip güncellenebilir
                                // Oranlar farklı ise güncelleme yapılabilir
                                await _matchRepository.Update(ite);
                            }


                        }
                        if (!string.Equals(ite.Teams, match.Teams))
                        {
                            if (ite.Odds1 == match.Odds1 && ite.OddsX == match.OddsX && ite.Odds2 == match.Odds2)
                            {
                                if ((ite.Tip == "? ?" || ite.Tip == "?") && (match.Tip != "? ?" && match.Tip != "?"))
                                {
                                    await _matchRepository.Update(ite);
                                }
                               
                            }
                            else
                            {
                               
                                await _matchRepository.Update(ite);
                            }
                        }
                    }

                    if (!matchExists)
                    {
                        await _matchRepository.Add(match);
                    }
                }
                else
                {
                    await _matchRepository.Add(match);
                }

                //List<Match> response = await _matchRepository.GetAll();
                //if (response.Count > 0)
                //{
                //    bool matchExists = false; // Flag to check if the match exists

                //    foreach (var ite in response)
                //    {
                //        // Check if the match exists based on teams
                //        if (string.Equals(ite.Teams, match.Teams))
                //        {
                //            matchExists = true;

                //            // Check if odds are the same
                //            if ((ite.Odds1 == match.Odds1 && ite.OddsX == match.OddsX && ite.Odds2 == match.Odds2) && ((ite.Tip == "? ?" || ite.Tip == "?") && (match.Tip != "? ?" || match.Tip != "?")))
                //            {

                //                await _matchRepository.Update(ite);
                //            }
                //            //if (item.Odds1 == match.Odds1 && item.OddsX == match.OddsX && item.Odds2 == match.Odds2)
                //            //{
                //            //    // Update existing match if necessary
                //            //    await _matchRepository.Update(item);
                //            //}
                //            else
                //            {
                //                // If the odds have changed, you might want to handle that separately
                //                // e.g., log a message, or update other relevant fields
                //            }

                //            break; // Exit loop if match is found
                //        }else if(!string.Equals(ite.Teams, match.Teams))
                //        {
                //            if ((ite.Odds1 == match.Odds1 && ite.OddsX == match.OddsX && ite.Odds2 == match.Odds2) && ((ite.Tip == "? ?" || ite.Tip == "?") && (match.Tip != "? ?" || match.Tip != "?")))
                //            {

                //                await _matchRepository.Update(ite);
                //            }
                //            else if ((ite.Odds1 != match.Odds1 && ite.OddsX != match.OddsX && ite.Odds2 != match.Odds2))
                //            {
                //                await _matchRepository.Add(match);

                //            }
                //        }
                //    }


                //    if (!matchExists)
                //    {
                //        await _matchRepository.Add(match);
                //    }
                //}
                //else
                //{
                //    // If no matches exist in the database, add the new match
                //    await _matchRepository.Add(match);
                //}








            }


        }

    }

    //web sitesinden maçları  veritabanına kayıt eder 
    public async Task GetMatchInWebSiteAndSaveDatabase(string url, string matchStatus)
    {

        var httpClient = new HttpClient();
        var cookie = _configuration.GetSection("CookieSettings");
       httpClient.DefaultRequestHeaders.Add("Cookie", $"timezone=Europe/Istanbul; timezone_offset=120; country=Turkey; country_code=TR; continent=Asia; notification_read=true; PHPSESSID={cookie.Value}; joinVisited=true; givenDate=2024-10-12 00:00:00; startDate=2024-10-11 22:00:00; endDate=2024-10-12 22:00:00; odds_format=EU");
        var html = await httpClient.GetStringAsync(url);

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);


        var matchListItems = htmlDocument.DocumentNode.SelectNodes("//div[@id='accordion' and contains(@class, 'nt-lists')]//div[contains(@class, 'nt-matches-list-item')]");

        if (matchListItems != null)
        {
            List<Match> response = await _matchRepository.GetAll();

            foreach (var item in matchListItems)
            {
                try
                {
                    string leagueName=" ";
                    // Lig adını çek
                    var leagueNode = item.SelectSingleNode(".//button[@type='button']");
                    if (leagueNode == null)
                    {
                        leagueName = "-";

                    }else if (leagueNode != null)
                    {
                        leagueName=leagueNode.InnerText.Trim();
                    }

                    // Maç bilgilerini seç
                    var rows = item.SelectNodes(".//tr[contains(@class, 'matchxd')]");
                    if (rows != null)
                    {
                        foreach (var row in rows)
                        {
                            var league = leagueName;
                            // Odds verilerini string olarak alıp formatlama yapıyoruz
                            string homeWinRaw = row.SelectSingleNode(".//td[contains(@class, 'td-1')]//div[@class='progress-bar']").InnerText.Trim();
                            string drawRaw = row.SelectSingleNode(".//td[contains(@class, 'td-x')]//div[@class='progress-bar']").InnerText.Trim();
                            string awayWinRaw = row.SelectSingleNode(".//td[contains(@class, 'td-2')]//div[@class='progress-bar']").InnerText.Trim();
                            var time = row.SelectSingleNode(".//td[contains(@class, 'td-data')]").InnerText.Trim();
                            var home = row.SelectSingleNode(".//div[contains(@class, 'score-left')]/span").InnerText.Trim();
                            var away = row.SelectSingleNode(".//div[contains(@class, 'score-right')]/span").InnerText.Trim();
                            var tip = row.SelectSingleNode(".//td[contains(@class, 'td-tip')]//p").InnerText.Trim();
                            var Goals = row.SelectSingleNode(".//td[contains(@class, 'td-goals')]//p").InnerText.Trim();
                            var gg = row.SelectSingleNode(".//td[contains(@class, 'td-btts')]//p").InnerText.Trim();
                            var betsTip = row.SelectSingleNode(".//td[contains(@class, 'td-best-tips')]//p").InnerText.Trim();
                            var trust = row.SelectSingleNode(".//td[contains(@class, 'td-trust')]").InnerText.Trim();

                            // Format odds and clean strings
                            string formattedHomeWinOdds = homeWinRaw.ToString();
                            string formattedDrawOdds = drawRaw.ToString();
                            string formattedAwayWinOdds = awayWinRaw.ToString();
                            string tipOdd = CleanString(row.SelectSingleNode(".//td[contains(@class,'td-tip')]//span")?.InnerText.Trim() ?? "Oran bilgisi yok");
                            string goalsOdds = CleanString(row.SelectSingleNode(".//td[contains(@class,'td-goals')]//span")?.InnerText.Trim() ?? "Oran bilgisi yok");
                            string bothTeamsToScoreOdds = CleanString(row.SelectSingleNode(".//td[contains(@class,'td-btts')]//span")?.InnerText.Trim() ?? "Oran bilgisi yok");
                            string bestTipOdds = CleanString(row.SelectSingleNode(".//td[contains(@class,'td-best-tips')]//span")?.InnerText.Trim() ?? "Oran bilgisi yok");

                            // Create match object
                            var team = home + "-" + away;
                            var odds1 = CleanString(formattedHomeWinOdds);
                            var oddsX = CleanString(formattedDrawOdds);
                            var odds2 = CleanString(formattedAwayWinOdds);
                            var leag = CleanString(league);
                            var tips = CleanString(tip);
                            var goal = CleanString(Goals);
                            var goalOd = CleanString(goalsOdds);
                            var g = CleanString(gg);
                            var gOd = CleanString(bothTeamsToScoreOdds);
                            var bTip = CleanString(betsTip);
                            var bTipOd = CleanString(bestTipOdds);
                            var trs = CleanString(trust);

                            Match match = new Match
                            {
                                MatchStatus = matchStatus,
                                MatchDate = time ?? "Match continue",
                                Teams = team,
                                League = leag,
                                Odds1 = odds1,
                                OddsX = oddsX,
                                Odds2 = odds2,
                                Tip = tips,
                                Goals = goal,
                                Gg = g,
                                BestTip = bTip,
                                Trust = trs,
                                BestTipOdd = bTipOd,
                                GgOdd = gOd,
                                GoalsOdd = goalOd,
                                TipOdd = tipOdd,
                                CreatedDate= DateOnly.FromDateTime(DateTime.Now.AddHours(2))

                        };

                            bool matchExists = false;

                            foreach (var ite in response)
                            {
                                if (string.Equals(ite.Teams, match.Teams))
                                {
                                    matchExists = true;

                                   
                                    if (ite.Odds1 == match.Odds1 && ite.OddsX == match.OddsX && ite.Odds2 == match.Odds2)
                                    {
                                        if ((ite.Tip == "? ?" || ite.Tip == "?") && (match.Tip != "? ?" && match.Tip != "?"))
                                        {
                                            await _matchRepository.Update(ite);
                                        }
                                    }
                                    else
                                    {
                                        await _matchRepository.Update(ite);
                                    }

                                   
                                }
                                if (!string.Equals(ite.Teams, match.Teams))
                                {
                                    if (ite.Odds1 == match.Odds1 && ite.OddsX == match.OddsX && ite.Odds2 == match.Odds2)
                                    {
                                        // Tip farklı ise, güncellenebilir
                                        if ((ite.Tip == "? ?" || ite.Tip == "?") && (match.Tip != "? ?" && match.Tip != "?"))
                                        {
                                            await _matchRepository.Update(ite);
                                        }

                                    }
                                    else
                                    {
                                       
                                        await _matchRepository.Update(ite);
                                    }
                                }
                            }

                            if (!matchExists)
                            {
                                await _matchRepository.Add(match);
                            }
                        }

                    }
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine("Error processing match item data: " + ex.Message);
                }
                //List<Match> response = await _matchRepository.GetAll();
                //foreach (var item in response)
                //{
                //    if (item == match)
                //    {
                //        continue;
                //    }
                //    else if (item != match)
                //    {
                //        await _matchRepository.Add(match);
                //    }
                //}


            }
        }
      

       


        //var cookie = _configuration.GetSection("CookieSettings");
        //var httpClient = new HttpClient();
        //httpClient.DefaultRequestHeaders.Add("Cookie", $"timezone=Europe/Istanbul; timezone_offset=120; country=Turkey; country_code=TR; continent=Asia; notification_read=true; PHPSESSID={cookie.Value}; joinVisited=true; givenDate=2024-10-12 00:00:00; startDate=2024-10-11 22:00:00; endDate=2024-10-12 22:00:00; odds_format=EU");

        //var html = await httpClient.GetStringAsync(url);
        //List<GetAllMatchResponse> getAlls = new();


        //var htmlDoc = new HtmlDocument();
        //htmlDoc.LoadHtml(html);

        //var matchRows = htmlDoc.DocumentNode.SelectNodes("//tbody/tr");

        //if (matchRows != null)
        //{
        //    foreach (var row in matchRows)
        //    {

        //        var timeNode = row.SelectSingleNode(".//p[contains(@class, 'score-time')]");
        //        var time = timeNode?.InnerText.Trim();


        //        var teamsNode = row.SelectSingleNode(".//td[contains(@class, 'td-match')]");

        //        var league = row.SelectSingleNode(".//button[@class='btn btn-link']");
        //        var odds1Node = row.SelectSingleNode(".//td[contains(@class, 'td-1')]");
        //        var oddsXNode = row.SelectSingleNode(".//td[contains(@class, 'td-x')]");
        //        var odds2Node = row.SelectSingleNode(".//td[contains(@class, 'td-2')]");
        //        var tips = row.SelectSingleNode(".//td[contains(@class,'td-tip')]");
        //        var goals = row.SelectSingleNode(".//td[contains(@class,'td-goals')]");
        //        var gg = row.SelectSingleNode(".//td[contains(@class,'td-btts')]");
        //        var bestTip = row.SelectSingleNode(".//td[contains(@class,'td-best-tip')]");
        //        var trust = row.SelectSingleNode(".//td[contains(@class,'td-trust')]");

        //        var team = CleanString(teamsNode?.InnerText);
        //        var odds1 = CleanString(odds1Node?.InnerText);
        //        var oddsX = CleanString(oddsXNode?.InnerText);
        //        var odds2 = CleanString(odds2Node?.InnerText);
        //        var leag = CleanString(league?.InnerText);
        //        var tip = CleanString(tips?.InnerText);
        //        var goal = CleanString(goals?.InnerText);
        //        var g = CleanString(gg?.InnerText);
        //        var bTip = CleanString(bestTip?.InnerText);
        //        var trs = CleanString(trust?.InnerText);

        //        Match match = new();
        //        match.MatchStatus = matchStatus;
        //        match.MatchDate = time;
        //        match.Teams = team.ToString();
        //        match.League = leag.ToString();
        //        match.Odds1 = odds1.ToString();
        //        match.OddsX = oddsX.ToString();
        //        match.Odds2 = odds2.ToString();
        //        match.Tip = tip.ToString();
        //        match.Goals = goal.ToString();
        //        match.Gg = g.ToString();
        //        match.BestTip = bTip.ToString();
        //        match.Trust = trs.ToString();
        //        if (match.MatchDate == null)
        //        {
        //            match.MatchDate = "Match continue";
        //        }
        //        List<Match> response =await _matchRepository.GetAll();
        //        foreach (var item in response)
        //        {
        //            if (item==match)
        //            {
        //                continue;
        //            }else if (item != match)
        //            {
        //                await _matchRepository.Add(match);
        //            }
        //        }






        //    }


        //}




    }

    public async Task<string> GetMyCookie()
    {
        var cookieSettings = _configuration.GetSection("CookieSettings");


        return cookieSettings.Value;
    }

   









    ////maç ve ligler
    //public async Task<List<GetAllMatchResponse>> GetAllMatchInWebSite(string url, string macthStatus)
    //{
    //   // "https://nerdytips.com/all-matches"
    //    var cookie = _configuration.GetSection("CookieSettings");
    //    var httpClient = new HttpClient();
    //    httpClient.DefaultRequestHeaders.Add("Cookie", $"timezone=Europe/Istanbul; timezone_offset=120; country=Turkey; country_code=TR; continent=Asia; notification_read=true; PHPSESSID={cookie.Value}; joinVisited=true; givenDate=2024-10-12 00:00:00; startDate=2024-10-11 22:00:00; endDate=2024-10-12 22:00:00; odds_format=EU");

    //    var html = await httpClient.GetStringAsync(url);
    //    List<GetAllMatchResponse> matches = new List<GetAllMatchResponse>();


    //    var htmlDoc = new HtmlDocument();
    //    htmlDoc.LoadHtml(html);

    //    var matchRows = htmlDoc.DocumentNode.SelectNodes("//tbody/tr");

    //    if (matchRows != null)
    //    {
    //        foreach (var row in matchRows)
    //        {

    //            var timeNode = row.SelectSingleNode(".//p[contains(@class, 'score-time')]");
    //            var time = timeNode?.InnerText.Trim() ?? "Saat bilgisi yok";
    //            var league= row.SelectSingleNode("//button[@class='btn btn-link']");

    //            var teamsNode = row.SelectSingleNode(".//td[contains(@class, 'td-match')]");


    //            var odds1Node = row.SelectSingleNode(".//td[contains(@class, 'td-1')]");
    //            var oddsXNode = row.SelectSingleNode(".//td[contains(@class, 'td-x')]");
    //            var odds2Node = row.SelectSingleNode(".//td[contains(@class, 'td-2')]");
    //            var tips = row.SelectSingleNode(".//td[contains(@class,'td-tip')]");
    //            var goals = row.SelectSingleNode(".//td[contains(@class,'td-goals')]");
    //            var gg = row.SelectSingleNode(".//td[contains(@class,'td-btts')]");
    //            var bestTip = row.SelectSingleNode(".//td[contains(@class,'td-best-tip')]");
    //            var trust = row.SelectSingleNode(".//td[contains(@class,'td-trust')]");

    //            var leag = CleanString(league?.InnerText);
    //            var team = CleanString(teamsNode?.InnerText);
    //            var odds1 = CleanString(odds1Node?.InnerText);
    //            var oddsX = CleanString(oddsXNode?.InnerText);
    //            var odds2 = CleanString(odds2Node?.InnerText);
    //            var tip = CleanString(tips?.InnerText);
    //            var goal = CleanString(goals?.InnerText);
    //            var g = CleanString(gg?.InnerText);
    //            var bTip = CleanString(bestTip?.InnerText);
    //            var trs = CleanString(trust?.InnerText);

    //            Match match = new();
    //            match.MatchStatus = macthStatus;
    //            match.League = leag;
    //            match.MatchDate = time;
    //            match.Teams = team;
    //            match.Odds1 = odds1;
    //            match.OddsX = oddsX;
    //            match.Odds2 = odds2;
    //            match.Tip = tip;
    //            match.Goals = goal;
    //            match.Gg = g;
    //            match.BestTip = bTip;
    //            match.Trust = trs;
    //            GetAllMatchResponse response = _mapper.Map<GetAllMatchResponse>(match);

    //            matches.Add(response);




    //        }

    //    }

    //    return matches;



    //}
}
