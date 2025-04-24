using AutoMapper;
using Bitredict.DataAccess.Abstract;
using Bitredict.DataAccess.EntityFramework;
using Bitredict.Dtos.Request;
using Bitredict.Dtos.Response;
using Bitredict.Models;
using Microsoft.EntityFrameworkCore;

namespace Bitredict.DataAccess.Concrete;

public class MatchRepository : EfRepositoryBase<Match, BaseDbContext>, IMatchRepository
{
    private readonly IMapper _mapper;
    public MatchRepository(BaseDbContext context) : base(context)
    {
    }
    public async Task AddBetOfTheDayMatch(List<Match> addBetOfTheDay)
    {
        using (var context = new BaseDbContext())
        {
            foreach (var item in addBetOfTheDay)
            {
                // Mevcut maçı al
                var existingMatch = await context.Matchs.FindAsync(item.Id);

                if (existingMatch != null)
                {
                    existingMatch.BetOfTheDayStatus = item.BetOfTheDayStatus;
                   
                    existingMatch.UpdatedDate = DateOnly.FromDateTime(DateTime.Now); 
                }
                else
                {
                    
                    await context.Matchs.AddAsync(item);
                }
            }

            await context.SaveChangesAsync(); 
        }
    }

    //public async Task AddBetOfTheDayMatch(List<Match> addBetOfTheDay)
    //{
    //    using (BaseDbContext context =new BaseDbContext())
    //    {
    //        foreach (var item in addBetOfTheDay)
    //        {
    //            var result =  context.Matchs
    //           .Where(m => m.Id == item.Id)
    //           .Select(d => new Match
    //           {
    //               Id = d.Id,
    //               BestTip = d.BestTip,
    //               BestTipOdd = d.BestTipOdd,
    //               BetOfTheDayStatus = item.BetOfTheDayStatus,
    //               CreatedDate = d.CreatedDate,
    //               DeletedDate = d.DeletedDate,
    //               Gg = d.Gg,
    //               GgOdd = d.GgOdd,
    //               Goals = d.Goals,
    //               GoalsOdd = d.GoalsOdd,
    //               League = d.League,
    //               MatchDate = d.MatchDate,
    //               MatchStatus = d.MatchStatus,
    //               Odds1 = d.Odds1,
    //               Odds2 = d.Odds2,
    //               OddsX = d.OddsX,
    //               Teams = d.Teams,
    //               Tip = d.Tip,
    //               TipOdd = d.TipOdd,
    //               Trust = d.Trust,
    //               UpdatedDate = d.UpdatedDate
    //           }).ToList();

    //            context.Add(result);
    //        }


    //        context.SaveChanges();
    //    }
    //}

    public async Task<List<Match>> GetBetOfTheDayBakersMatch(DateOnly dateOnly)
    {
        using (BaseDbContext context = new BaseDbContext())
        {
          
            var result =await context.Matchs
                .Where(d => d.MatchStatus == "Bet Of The Day" && d.BetOfTheDayStatus == "Bankers" && d.CreatedDate==dateOnly)
                .Select(m=>new Match
                {
                    BetOfTheDayStatus = m.BetOfTheDayStatus,
                    Id = m.Id,
                    MatchStatus= m.MatchStatus,
                    BestTip = m.BestTip,
                    BestTipOdd  = m.BestTipOdd,
                    CreatedDate = m.CreatedDate,
                    DeletedDate = m.DeletedDate,
                    Gg=m.Gg,
                    GgOdd=m.GgOdd,
                    Goals=m.Goals,
                    GoalsOdd=m.GoalsOdd,
                    League=m.League,
                    MatchDate=m.MatchDate,
                    Odds1   = m.Odds1,
                    Odds2 = m.Odds2,
                    OddsX = m.OddsX,
                    Teams = m.Teams,
                    Tip = m.Tip,
                    TipOdd = m.TipOdd,
                    Trust=m.Trust,
                    UpdatedDate = m.UpdatedDate
                    
                }).ToListAsync();
            return result;
            
            
        }

    }
    public async Task<List<Match>> GetBetOfTheDaySlipOfTheDayMatch(DateOnly dateOnly)
    {
        using (BaseDbContext context = new BaseDbContext())
        {
            var result =await context.Matchs
                .Where(d => d.MatchStatus == "Bet Of The Day" && d.BetOfTheDayStatus == "Slip Of The Day" && d.CreatedDate==dateOnly)
                .Select(m => new Match
                {
                    BetOfTheDayStatus = m.BetOfTheDayStatus,
                    Id = m.Id,
                    MatchStatus = m.MatchStatus,
                    BestTip = m.BestTip,
                    BestTipOdd = m.BestTipOdd,
                    CreatedDate = m.CreatedDate,
                    DeletedDate = m.DeletedDate,
                    Gg = m.Gg,
                    GgOdd = m.GgOdd,
                    Goals = m.Goals,
                    GoalsOdd = m.GoalsOdd,
                    League = m.League,
                    MatchDate = m.MatchDate,
                    Odds1 = m.Odds1,
                    Odds2 = m.Odds2,
                    OddsX = m.OddsX,
                    Teams = m.Teams,
                    Tip = m.Tip,
                    TipOdd = m.TipOdd,
                    Trust = m.Trust,
                    UpdatedDate = m.UpdatedDate

                }).ToListAsync();
            return result;

        }
    }
    public async Task<List<Match>> GetBetOfTheDayMatchByDate(DateOnly date)
    {
        using (BaseDbContext context = new BaseDbContext())
        {
            try
            {
                var matches = await context.Matchs
     .Where(d => d.CreatedDate == date && d.MatchStatus== "Bet Of The Day")
     .Select(d => new Match
     {
         Id = d.Id,
         CreatedDate = d.CreatedDate,
         DeletedDate = d.DeletedDate,
         MatchStatus = "Bet Of The Day",
         UpdatedDate = d.UpdatedDate,
         Teams = d.Teams,
         MatchDate = d.MatchDate,
         BestTip = d.BestTip,
         Gg = d.Gg,
         Goals = d.Goals,
         Odds1 = d.Odds1,
         Odds2 = d.Odds2,
         OddsX = d.OddsX,
         Tip = d.Tip,
         Trust = d.Trust,
     }).ToListAsync();


                return matches;
            }
            catch (Exception e)
            {

                throw e;
            }



        }

    }

   

    public async Task<List<Match>> GetByDateMatch(DateOnly date)
    {


        using (BaseDbContext context = new BaseDbContext())
        {
            try
            {
                var matches = await context.Matchs
     .Where(d => d.CreatedDate == date && d.MatchStatus== "All Match")
     .Select(d => new Match
     {
         Id = d.Id,
         League=d.League,
         CreatedDate = d.CreatedDate,
         DeletedDate = d.DeletedDate,
         MatchStatus = "All Match",
         UpdatedDate = d.UpdatedDate,
         Teams = d.Teams,
         MatchDate = d.MatchDate,
         BestTip = d.BestTip,
         BestTipOdd = d.BestTipOdd,
         Gg = d.Gg,
         GgOdd = d.GgOdd,
         Goals = d.Goals,
         GoalsOdd = d.GoalsOdd,
         Odds1 = d.Odds1,
         Odds2 = d.Odds2,
         OddsX = d.OddsX,
         Tip = d.Tip,
         TipOdd = d.TipOdd,
         Trust = d.Trust,
     }).ToListAsync();


                return matches;
            }
            catch (Exception e)
            {

                throw e;
            }



        }

    }

    public async Task<List<Match>> GetBetOfTheDayBankersAndSlipOfTheDayMatch(DateOnly dateOnly)
    {
        using (BaseDbContext context = new BaseDbContext())
        {
            var result = await context.Matchs
                .Where(d => d.MatchStatus == "Bet Of The Day" && d.BetOfTheDayStatus == "Bankers And Slip Of The Day" && d.CreatedDate == dateOnly)
                .Select(m => new Match
                {
                    BetOfTheDayStatus = m.BetOfTheDayStatus,
                    Id = m.Id,
                    MatchStatus = m.MatchStatus,
                    BestTip = m.BestTip,
                    BestTipOdd = m.BestTipOdd,
                    CreatedDate = m.CreatedDate,
                    DeletedDate = m.DeletedDate,
                    Gg = m.Gg,
                    GgOdd = m.GgOdd,
                    Goals = m.Goals,
                    GoalsOdd = m.GoalsOdd,
                    League = m.League,
                    MatchDate = m.MatchDate,
                    Odds1 = m.Odds1,
                    Odds2 = m.Odds2,
                    OddsX = m.OddsX,
                    Teams = m.Teams,
                    Tip = m.Tip,
                    TipOdd = m.TipOdd,
                    Trust = m.Trust,
                    UpdatedDate = m.UpdatedDate

                }).ToListAsync();
            return result;

        }
    }

    public async Task<Match> GetMatchById(int id)
    {
        using (BaseDbContext context=new BaseDbContext())
        {
            var result=await context.Matchs.FindAsync(id);
            return result;
        }
    }

    public async Task UpdateMacthScore(int id, string macth,string matchDate)
    {
        using (BaseDbContext context = new BaseDbContext())
        {
          var result=await  context.Matchs.FirstOrDefaultAsync(i => i.Id==id);
           if (result != null)
            {
                result.Teams = macth;
                result.MatchDate = matchDate;
               // context.Matchs.Attach(result);
                context.Entry(result).Property(m=>m.Teams).IsModified=true;
                context.Entry(result).Property(r => r.League).IsModified = false;
                context.Entry(result).Property(r => r.BetOfTheDayStatus).IsModified = false;
                context.Entry(result).Property(r => r.MatchStatus).IsModified = false;
                context.Entry(result).Property(r => r.Odds1).IsModified = false;
                context.Entry(result).Property(r => r.OddsX).IsModified = false;
                context.Entry(result).Property(r => r.Odds2).IsModified = false;
                context.Entry(result).Property(r => r.Tip).IsModified = false;
                context.Entry(result).Property(r => r.TipOdd).IsModified = false;
                context.Entry(result).Property(r => r.Goals).IsModified = false;
                context.Entry(result).Property(r => r.GoalsOdd).IsModified = false;
                context.Entry(result).Property(r => r.Gg).IsModified = false;
                context.Entry(result).Property(r => r.GgOdd).IsModified = false;
                context.Entry(result).Property(r => r.BestTip).IsModified = false;
                context.Entry(result).Property(r => r.BestTipOdd).IsModified = false;
                context.Entry(result).Property(r => r.Trust).IsModified = false;
                context.Entry(result).Property(r => r.MatchDate).IsModified = true;
                context.SaveChanges();
            }
        }
    }
}


