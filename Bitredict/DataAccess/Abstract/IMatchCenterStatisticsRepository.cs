using Bitredict.Models;

namespace Bitredict.DataAccess.Abstract;

public interface IMatchCenterStatisticsRepository:IRepository<MatchCenterStatistics>
{
    public Task<MatchCenterStatistics> GetMatchCenterStatisticsByDate(DateOnly dateOnly);
}
