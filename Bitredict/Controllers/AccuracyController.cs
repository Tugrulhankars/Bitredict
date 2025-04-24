using Bitredict.Dtos;
using Bitredict.Dtos.Request;
using Bitredict.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bitredict.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccuracyController : ControllerBase
{
    private readonly IAccuracyService _accuracyService;
    public AccuracyController(IAccuracyService accuracyService)
    {
        _accuracyService = accuracyService;
    }

    [HttpPost("addAccuracy")]
    public IActionResult Add([FromBody] AddAccuracyRequest request)
    {
        _accuracyService.AddAccuracy(request);
        return Ok();
    }

    [HttpGet("getAccuracyByDate")]
    public async Task<IActionResult> GetAccuracyByDate([FromQuery] DateDto date)
    {
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
        var result=await _accuracyService.GetAccuracyByDate(dateOnly);
        return Ok(result);



    }
}
