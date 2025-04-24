using Bitredict.Dtos.Request;

namespace Bitredict.Services.Abstracts;

public interface IAccuracyService
{
    public Task AddAccuracy(AddAccuracyRequest request);

    Task< double> GetAccuracyByDate(DateOnly dateOnly);
}
