using Bitredict.DataAccess.Abstract;
using Bitredict.DataAccess.EntityFramework;
using Bitredict.Models;
using Microsoft.EntityFrameworkCore;

namespace Bitredict.DataAccess.Concrete;

public class AccuracyRepository : EfRepositoryBase<Accuracy, BaseDbContext>, IAccuracyRepository
{
    public AccuracyRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<Accuracy> GetAccuracyByDate(DateOnly dateOnly)
    {
        using (BaseDbContext context = new BaseDbContext())
        {
            var result =await context.Accuracies
                 .Where(d => d.CreatedDate == dateOnly)
                 .Select(m => new Accuracy
                 {
                     AccuracyId = m.AccuracyId,
                     CreatedDate = m.CreatedDate,
                     AccuracyValue = m.AccuracyValue,
                 }).FirstOrDefaultAsync();

            return result;
        }
    }
}
