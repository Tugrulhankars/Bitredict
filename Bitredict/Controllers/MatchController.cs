using Bitredict.Dtos;
using Bitredict.Dtos.Request;
using Bitredict.Dtos.Response;
using Bitredict.Services.Abstracts;
using Bitredict.Services.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bitredict.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MatchController : ControllerBase
{
    private readonly IMatchService _matchService;
    private readonly IConfiguration _configuration;
    public MatchController(IMatchService matchService, IConfiguration configuration)
    {
        _matchService = matchService;
        _configuration = configuration;

    }

    //veritaba'nından maçları getirir
    [HttpGet("getAllMatchInDatabase")]

    public async Task<IActionResult> GetAll()
    {
        var result = await _matchService.GetAllMatchInDatabase();
        return Ok(result);
    }

    // https://nerdytips.com/bet-of-the-day adrese gider maçları veritabanına kayıt eder
    [HttpPost("addBetOfTheDayMatchInWebSite")]
    public async Task<IActionResult> AddBetOfTheDayMatchInWebSite()
    {
        await _matchService.AddBetOfTheDayMatchDatabase();
        return Ok();
    }

    //custom maç güncellemek için
    [HttpPut("updateMatch")]
    public async Task<IActionResult> Update(UpdateMatchRequest request)
    {
        var result = await _matchService.UpdateMatch(request);
        return Ok(result);
    }


    //custom maç eklemek için 
    [HttpPost("addCustomMatch")]
    public async Task<IActionResult> Add(AddMatchRequest addMatchRequest)
    {
        var result = await _matchService.AddMatch(addMatchRequest);
        return Ok(result);
    }

    // https://nerdytips.com/all-matches adrese gider maçları veritabanına kayıt eder
    [HttpPost("addAllMatchDatabase")]
    public async Task<IActionResult> GetAllMatchInWebSite()
    {
        await _matchService.AddAllMacthDatabase();
        return Ok();
    }

    //https://nerdytips.com/bet-of-the-day adresdeki maçları getirir ve liste şeklinde geri döner 
    // not veritabanına kayıt yapmaz
    [HttpGet("getDirectBetOfTheDayMatch")]
    public async Task<IActionResult> GetDirectBetOfTheDayMatch()
    {
        var result = await _matchService.GetDirectBetOfTheDayMatch();
        return Ok(result);
    }


    //nerdytips sitesinden doğrudan maçları getirir 
    [HttpGet("getDirectAllMacth")]
    public async Task<IActionResult> GetDirectAllMacthInWebSite()
    {
        var result = await _matchService.GetAllMatchInWebSite();
        return Ok(result);
    }

    //parametreye verilen tarihteki maçları getirir

    [HttpGet("getByDateMatchInDatabase")]
    public async Task<IActionResult> GetByDateMatch([FromQuery] DateDto dateDto)
    {
        var dateOnly = new DateOnly(dateDto.Year, dateDto.Month, dateDto.Day);
        List<GetAllMatchResponse> result = await _matchService.GetByDateMatch(dateOnly);
        return Ok(result);
    }

    //verdiğimiz id'ye sahip maçları siler
    [HttpDelete("deleteById")]
    public async Task<IActionResult> Delete([FromBody] DeleteMatchRequest delete)
    {
        var result = _matchService.DeleteMatch(delete);
        return Ok(result);
    }


    //yeni cookie değeri atamamızı sağlar
    [HttpPost("saveNewCookieValue")]

    public async Task<IActionResult> SaveCookie([FromForm] string cookieValue)
    {
        _matchService.SetNewCookieValue(cookieValue);
        return Ok();
    }

    //sahip olduğumuz cookie değerini getirir
    [HttpGet("getMyCookie")]
    public async Task<IActionResult> GetMyCookie()
    {
        var cookie = await _matchService.GetMyCookie();


        return Ok(new { CookieValue = cookie });
    }

    [HttpGet("getBetOfTheDayMatchByDate")]
    public async Task<IActionResult> GetBetOfTheDayMatchByDate([FromQuery] DateDto date)
    {
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
        List<GetBetOfTheDatMatchReponse> result =await _matchService.GetBetOfTheDayMatchByDate(dateOnly);
        return Ok(result);
    }

    [HttpGet("getBetOfTheDayMatch")]
    public async Task<IActionResult> GetBetOfTheDayMatch()
    {
        var response = await _matchService.GetBetOfTheDayMatch();
        return Ok(response);
    }

    [HttpPost("addBetOfTheDatMatch")]
    public async Task<IActionResult> AddBetOfTheDatMatch([FromBody] List<AddBetOfTheDay> addBetOfTheDay)
    {
        await _matchService.AddBetOfTheDay(addBetOfTheDay);
        return Ok(); 
    }

    [HttpGet("getBetOfTheDayBakers")]
    public async Task<IActionResult> GetBetOfTheDayBakersMacth([FromQuery] DateDto date) 
    {
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);

        var response =   await  _matchService.GetBetOfTheDayBakersMatch(dateOnly);
        return Ok(response);
    }
    [HttpGet("getBetOfTheDaySlipOfTheDayMatch")]
    public async Task<IActionResult> GetBetOfTheDaySlipOfTheDayMatch([FromQuery] DateDto date)
    {
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);

        var response =   await _matchService.GetBetOfTheDaySlipOfTheDayMatch(dateOnly);
        return Ok(response);
    }

    [HttpGet("getBetOfTheDayBankersAndSlipOfTheDayMatch")]
    public async Task<IActionResult> GetBetOfTheDayBankersAndSlipOfTheDay([FromQuery] DateDto date)
    {
        var dateOnly = new DateOnly(date.Year,date.Month,date.Day);
        var response = await _matchService.GetBetOfTheDayBankersAndSlipOfTheDay(dateOnly);
        return Ok(response);
    }

    //[HttpGet("deneme")]
    //public async Task<IActionResult> deneme()
    //{

    //}
}
