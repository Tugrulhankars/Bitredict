using AutoMapper;
using Bitredict.DataAccess.Abstract;
using Bitredict.Dtos.Request;
using Bitredict.Models;
using Bitredict.Services.Abstracts;

namespace Bitredict.Services.Manager;

public class AccuracyManager : IAccuracyService
{
    private readonly IAccuracyRepository _accuracyRepository;
    private readonly IMapper _mapper;
    public AccuracyManager(IAccuracyRepository accuracyRepository,IMapper mapper)
    {
        _accuracyRepository = accuracyRepository;   
        _mapper = mapper;
    }
    public async Task AddAccuracy(AddAccuracyRequest request)
    {
        Accuracy accuracy = new Accuracy();
        accuracy.AccuracyId = request.AccuracyId;
        accuracy.AccuracyValue = request.AccuracyValue;
        accuracy.CreatedDate = DateOnly.FromDateTime(DateTime.Now.AddHours(2));
        await _accuracyRepository.Add(accuracy);
    }

    public async Task<double> GetAccuracyByDate(DateOnly dateOnly)
    {
        Accuracy result =await _accuracyRepository.GetAccuracyByDate(dateOnly);
        return result.AccuracyValue;
       
    }
}
