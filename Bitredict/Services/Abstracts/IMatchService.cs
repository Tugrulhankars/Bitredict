using Bitredict.Dtos.Request;
using Bitredict.Dtos.Response;

namespace Bitredict.Services.Abstracts;

public interface IMatchService
{
    public Task<AddMatchResponse> AddMatch(AddMatchRequest addMatchRequest);
    public Task<UpdateMatchResponse> UpdateMatch(UpdateMatchRequest updateMatchRequest);
    public Task<List<GetAllMatchResponse>> GetAllMatchInDatabase();
    public Task<List<GetAllMatchResponse>> GetAllMatchInWebSite();
    public Task<List<GetBetOfTheDatMatchReponse>> GetBetOfTheDayMatch();
    public Task AddBetOfTheDayMatchDatabase();
    public Task AddAllMacthDatabase();
    public Task<List<GetAllMatchResponse>> GetDirectBetOfTheDayMatch();
    public Task DeleteMatch(DeleteMatchRequest deleteMatch);
    public Task<List<GetAllMatchResponse>> GetByDateMatch(DateOnly dateMatch);
    public Task<List<GetBetOfTheDatMatchReponse>> GetBetOfTheDayMatchByDate(DateOnly dateMatch);
    public Task SetNewCookieValue(string PHPSESSID);
    public Task<string> GetMyCookie();
    public Task AddBetOfTheDay(List<AddBetOfTheDay> addBetOfTheDay);
    public Task<List<GetBetOfTheDayBakers>> GetBetOfTheDayBakersMatch(DateOnly dateOnly);
    public Task<List<GetBetOfTheDaySlipOfTheDay>> GetBetOfTheDaySlipOfTheDayMatch(DateOnly dateOnly);
    public Task<List<GetBetOfTheDayBankersAndSlipOfTheDay>> GetBetOfTheDayBankersAndSlipOfTheDay(DateOnly dateOnly);

}
