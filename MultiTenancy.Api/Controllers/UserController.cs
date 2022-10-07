using Microsoft.AspNetCore.Mvc;
using MultiTenancy.Application.DTOs.Request;
using MultiTenancy.Application.DTOs.Response;
using MultiTenancy.Application.Interfaces.Services;

namespace MultiTenancy.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private IIdentityService _identityService;

    public UserController(IIdentityService identityService) => _identityService = identityService;

    [HttpPost("register")]
    public async Task<ActionResult<RegisterUserResponse>> Register(UserRegisterRequest userCad)
    {
        if (!ModelState.IsValid) return BadRequest();

        var result = await _identityService.RegisterUser(userCad);

        if (result.Success)
            return Ok(result);

        else if (result.Errors.Count > 0)
            return BadRequest(result);
        
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPost("login")]
    public async Task<ActionResult<RegisterUserResponse>> Login(UserLoginRequest userLogin)
    {
        if (!ModelState.IsValid) return BadRequest();

        var result = await _identityService.Login(userLogin);

        if (result.Success) return Ok(result);
        
        return Unauthorized(result);
    }
}