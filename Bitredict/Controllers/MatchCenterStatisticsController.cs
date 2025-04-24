using Bitredict.Dtos;
using Bitredict.Dtos.Request;
using Bitredict.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bitredict.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MatchCenterStatisticsController : ControllerBase
{
    private readonly IMatchCenterStatisticsService _matchCenterStatisticsService;
    public MatchCenterStatisticsController(IMatchCenterStatisticsService matchCenterStatisticsService)
    {
        _matchCenterStatisticsService = matchCenterStatisticsService;
    }


    [HttpPost("addMatchCenterStatistics")]
    public async Task<IActionResult> AddMacthCenterStatistics([FromBody] AddMatchCenterStatisticsRequest addMatchCenterStatisticsRequest)
    {
        await _matchCenterStatisticsService.AddStatistics(addMatchCenterStatisticsRequest);
        return Ok();
    }

    [HttpGet("getMatchCenterStatistics")]
    public async Task<IActionResult> GetMacthCenterStatistics([FromQuery] DateDto date)
    {
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
        var response=await _matchCenterStatisticsService.GetStatistics(dateOnly);
        return Ok(response);
    }
}
