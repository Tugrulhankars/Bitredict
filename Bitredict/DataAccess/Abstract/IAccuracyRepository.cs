using Bitredict.Models;

namespace Bitredict.DataAccess.Abstract;

public interface IAccuracyRepository:IRepository<Accuracy>
{
   Task<Accuracy> GetAccuracyByDate(DateOnly dateOnly);
}
