using Bitredict.Services.Abstracts;
using Quartz;

namespace Bitredict.Job;

public class MatchCenterStatisticsJob : IJob
{
    private readonly IMatchCenterStatisticsService _matchCenterStatisticsService;
    public MatchCenterStatisticsJob(IMatchCenterStatisticsService matchCenterStatisticsService)
    {
        _matchCenterStatisticsService = matchCenterStatisticsService;
    }
    public  Task Execute(IJobExecutionContext context)
    {
       // _matchCenterStatisticsService.AddStatistics();
        return Task.CompletedTask;
    }
}
