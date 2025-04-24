using Bitredict.DataAccess.Abstract;
using Bitredict.DataAccess.EntityFramework;
using Bitredict.Models;

namespace Bitredict.DataAccess.Concrete;

public class MatchCenterStatisticsRepository : EfRepositoryBase<MatchCenterStatistics, BaseDbContext>, IMatchCenterStatisticsRepository
{
    public MatchCenterStatisticsRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<MatchCenterStatistics> GetMatchCenterStatisticsByDate(DateOnly dateOnly)
    {
        using (BaseDbContext context=new BaseDbContext())
        {
            var result = context.MatchCenterStatistics.
                Where(d => d.CreatedDate == dateOnly)
                .Select(m => new MatchCenterStatistics
                {
                    CreatedDate = m.CreatedDate,
                    Id = m.Id,
                    Predict=m.Predict,
                    Upcomig=m.Upcomig,
                    Won=m.Won,
                });

            return (MatchCenterStatistics)result;
        }
    }
}
