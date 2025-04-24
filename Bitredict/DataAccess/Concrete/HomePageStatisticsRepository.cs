using Bitredict.DataAccess.Abstract;
using Bitredict.DataAccess.EntityFramework;
using Bitredict.Models;

namespace Bitredict.DataAccess.Concrete;

public class HomePageStatisticsRepository : EfRepositoryBase<HomePageStatistics, BaseDbContext>, IHomePageStatisticsRepository
{
    public HomePageStatisticsRepository(BaseDbContext context) : base(context)
    {
    }
}
