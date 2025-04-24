using Bitredict.Dtos.Request;
using Bitredict.Dtos.Response;

namespace Bitredict.Services.Abstracts;

public interface IMatchCenterStatisticsService
{
    public Task AddStatistics(AddMatchCenterStatisticsRequest addMatchCenterStatisticsRequest);
    public Task<GetMatchCenterStatistics> GetStatistics(DateOnly dateOnly);
    //public Task UpdateStatistics(AddMatchCenterStatisticsRequest updateMatchCenterStatisticsRequest);
}
