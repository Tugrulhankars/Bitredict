using Bitredict.Dtos.Request;
using Bitredict.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bitredict.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("createNewUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
    {
        var result = await _userService.CreateUser(createUserRequest);
        return Ok(result);
    }

    [HttpGet("getUserByWalletPublicAddress")]
    public async Task<IActionResult> GetUser([FromQuery] string walletPublicAddress)
    {
        var result = await _userService.GetUser(walletPublicAddress);
        return Ok(result);
    }


}
