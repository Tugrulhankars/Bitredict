using Bitredict.Dtos.Request;
using Bitredict.Dtos.Response;
using Bitredict.Models;

namespace Bitredict.DataAccess.Abstract;

public interface IMatchRepository : IRepository<Match>
{

    public Task<List<Match>> GetByDateMatch(DateOnly date);
    public Task<List<Match>> GetBetOfTheDayMatchByDate(DateOnly date);
    public Task AddBetOfTheDayMatch(List<Match> addBetOfTheDay);
    public Task<List<Match>> GetBetOfTheDayBakersMatch(DateOnly dateOnly);
    public Task<List<Match>> GetBetOfTheDaySlipOfTheDayMatch(DateOnly dateOnly);
    public Task<List<Match>> GetBetOfTheDayBankersAndSlipOfTheDayMatch(DateOnly dateOnly);
    public Task<Match> GetMatchById(int id);

    public Task UpdateMacthScore(int id,string macth,string matchDate);
}