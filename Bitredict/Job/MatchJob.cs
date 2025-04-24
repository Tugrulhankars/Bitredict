using Bitredict.Services.Abstracts;
using Quartz;

namespace Bitredict.Job;

public class MatchJob : IJob
{
    private readonly IMatchService _matchService;
    public MatchJob(IMatchService matchService)
    {
        _matchService = matchService;
    }
    public  Task Execute(IJobExecutionContext context)
    {
         _matchService.AddAllMacthDatabase();
        _matchService.AddBetOfTheDayMatchDatabase();
        return Task.CompletedTask;
    }
}
