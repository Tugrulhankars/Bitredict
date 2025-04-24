using Bitredict.DataAccess.Abstract;
using Bitredict.Dtos;
using Bitredict.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bitredict.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomePageStatisticsController : ControllerBase
{
    private readonly IHomePageStatisticsService _homePageStatisticsService;
    public HomePageStatisticsController(IHomePageStatisticsService homePageStatisticsService)
    {
        _homePageStatisticsService = homePageStatisticsService;
    }

    [HttpPost("addHomePageStatistics")]
    public async Task<IActionResult> AddHomePageStatistics()
    {
         await _homePageStatisticsService.AddHomePageStatistics();
        return Ok();
    }

    [HttpGet("getHomePageStatistics")]
    public async Task<IActionResult> GetHomePageStatistics()
    {
        var response = await _homePageStatisticsService.GetHomePageStatistics();
        return Ok(response);
    }
}
