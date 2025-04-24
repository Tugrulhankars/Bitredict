using AutoMapper;
using Bitredict.DataAccess.Abstract;
using Bitredict.DataAccess.Concrete;
using Bitredict.Dtos.Response;
using Bitredict.Models;
using Bitredict.Services.Abstracts;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Bitredict.Services.Manager;

public class HomePageStatisticsManager : IHomePageStatisticsService
{
    private readonly IHomePageStatisticsRepository _homePageStatisticsRepository;
    private readonly IMapper _mapper;
    public HomePageStatisticsManager(IHomePageStatisticsRepository homePageStatisticsRepository,IMapper mapper)
    {
        _homePageStatisticsRepository = homePageStatisticsRepository;
        _mapper = mapper;
    }
    public async Task AddHomePageStatistics()
    {
        var statistics = await GetHomePageStatisticsInWebSite();
        List<HomePageStatistics> response = await _homePageStatisticsRepository.GetAll();

        if (response.Count == 0)
        {
            await _homePageStatisticsRepository.Add(statistics);
        }
        else
        {
            foreach (var item in response)
            {
                item.PredictedToday=statistics.PredictedToday;
                item.MathcesWon = statistics.MathcesWon;
                item.BankersRate = statistics.BankersRate;
               

                try
                {
                    await _homePageStatisticsRepository.Update(item);
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw new InvalidOperationException("The statistics have been modified by another user.");
                }
            }
        }
        
    }

    public async Task<GetHomePageStatistics> GetHomePageStatistics()
    {
        List<HomePageStatistics> response=await _homePageStatisticsRepository.GetAll();
        List<GetHomePageStatistics> statistics=new List<GetHomePageStatistics>();
        GetHomePageStatistics statistics1 = new GetHomePageStatistics();
        foreach (var item in response)
        {
            statistics1.PredictedToday = item.PredictedToday;
            statistics1.MathcesWon = item.MathcesWon;
            statistics1.BankersRate = item.BankersRate;
            statistics.Add(statistics1);
        }

        
        return statistics1;
    }


    public async Task<HomePageStatistics> GetHomePageStatisticsInWebSite()
    {
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync("https://nerdytips.com");
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);
        var predictedTodayValueNode = doc.DocumentNode.SelectSingleNode("//div[@class='single-statics no-border']//span[@class='counter']");


        var predictToday = CleanString(predictedTodayValueNode?.InnerText);
           
        

      
        var bankersRateValueNode = doc.DocumentNode.SelectSingleNode("(//div[@class='text-box']//span[@class='counter'])[2]");

      
            var bankersRateValue = CleanString(bankersRateValueNode?.InnerText);
          

        
        var matchesWonValueNode = doc.DocumentNode.SelectSingleNode("(//div[@class='text-box']//span[@class='counter'])[3]");


        var matchesWonValue = CleanString(matchesWonValueNode?.InnerText);

        HomePageStatistics homePageStatistics = new();
        homePageStatistics.PredictedToday = predictToday.ToString();
        homePageStatistics.BankersRate = bankersRateValue.ToString();
        homePageStatistics.MathcesWon = matchesWonValue.ToString();
        
        return homePageStatistics;
       

    }

    public static string CleanString(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return Regex.Replace(input, @"\s+", " ").Trim();
    }

   
}
