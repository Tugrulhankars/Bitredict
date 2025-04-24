using Bitredict.Dtos.Response;

namespace Bitredict.Services.Abstracts;

public interface IHomePageStatisticsService
{
    Task AddHomePageStatistics();
    Task<GetHomePageStatistics> GetHomePageStatistics();
}
