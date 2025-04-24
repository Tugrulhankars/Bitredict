using Bitredict.Services.Abstracts;
using Quartz;

namespace Bitredict.Job;

public class HomePageStatisticsJob : IJob
{
    private readonly IHomePageStatisticsService _homePageStatisticsService;
    public HomePageStatisticsJob(IHomePageStatisticsService homePageStatisticsService)
    {
        _homePageStatisticsService = homePageStatisticsService;

    }
    public  Task Execute(IJobExecutionContext context)
    {
         _homePageStatisticsService.AddHomePageStatistics();
        return Task.CompletedTask;
    }
}
