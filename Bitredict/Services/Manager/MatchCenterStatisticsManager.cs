using AutoMapper;
using Bitredict.DataAccess.Abstract;
using Bitredict.DataAccess.Concrete;
using Bitredict.Dtos.Request;
using Bitredict.Dtos.Response;
using Bitredict.Models;
using Bitredict.Services.Abstracts;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Bitredict.Services.Manager;

public class MatchCenterStatisticsManager : IMatchCenterStatisticsService
{
    private readonly IMatchCenterStatisticsRepository _matchCenterStatisticsRepository;
    private readonly IMapper _mapper;
    public MatchCenterStatisticsManager(IMatchCenterStatisticsRepository matchCenterStatisticsRepository,IMapper mapper)
    {
        _matchCenterStatisticsRepository = matchCenterStatisticsRepository;
        _mapper = mapper;
    }
   
    public async Task AddStatistics(AddMatchCenterStatisticsRequest addMatchCenterStatisticsRequest)
    {
        
        MatchCenterStatistics matchCenterStatistics=new MatchCenterStatistics();
        matchCenterStatistics.Predict=addMatchCenterStatisticsRequest.Predict;
        matchCenterStatistics.Upcomig=addMatchCenterStatisticsRequest.Upcomig;
        matchCenterStatistics.Won=addMatchCenterStatisticsRequest.Won;
        matchCenterStatistics.CreatedDate = DateOnly.FromDateTime(DateTime.Now.AddHours(2));

        await _matchCenterStatisticsRepository.Add(matchCenterStatistics);
        //var statistics = await GetMacthCenterStatisticsInWebSite();
        //List<MatchCenterStatistics> response = await _matchCenterStatisticsRepository.GetAll();

        //if (response.Count == 0)
        //{
        //    await _matchCenterStatisticsRepository.Add(statistics);
        //}
        //else
        //{
        //    foreach (var item in response)
        //    {
        //        item.Predict = statistics.Predict;
        //        item.Upcomig = statistics.Upcomig;
        //        item.Won = statistics.Won;


        //        try
        //        {
        //            await _matchCenterStatisticsRepository.Update(item);
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {

        //            throw new InvalidOperationException("The statistics have been modified by another user.");
        //        }
        //    }
        //}
    }

    public async Task<GetMatchCenterStatistics> GetStatistics(DateOnly dateOnly)
    {
        List<MatchCenterStatistics> response = await _matchCenterStatisticsRepository.GetAll();
        List<GetMatchCenterStatistics> statis = new List<GetMatchCenterStatistics>();
        GetMatchCenterStatistics statistics1 = new GetMatchCenterStatistics();
        foreach (var item in response)
        {
            statistics1.Predict= item.Predict;
            statistics1.Upcomig = item.Upcomig;
            statistics1.Won = item.Won;
            statis.Add(statistics1);
           
        }


        return statistics1;
    }


    public async Task<MatchCenterStatistics> GetMacthCenterStatisticsInWebSite()
    {
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync("https://nerdytips.com/all-matches");
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);


        var predictedNode = doc.DocumentNode.SelectSingleNode("//span[@id='predicted_count']");

        // Upcoming değeri
        var upcomingNode = doc.DocumentNode.SelectSingleNode("//span[@id='notstarted_count']");

        // Won Matches değeri
        var wonMatchesNode = doc.DocumentNode.SelectSingleNode("//span[@id='success_rate']");

        var predict=CleanString(predictedNode?.InnerText);
        var upcoming=CleanString(upcomingNode?.InnerText);
        var won=CleanString(wonMatchesNode?.InnerText);

        MatchCenterStatistics matchCenter = new();
        matchCenter.Predict = predict.ToString();
        matchCenter.Upcomig= upcoming.ToString();
        matchCenter.Won= won.ToString();
        
        return matchCenter;


     
    }

    public static string CleanString(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return Regex.Replace(input, @"\s+", " ").Trim();
    }

   
}
