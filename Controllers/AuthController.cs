using Inventory.Models;
using Inventory.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Inventory.Services;

namespace Inventory.Controllers;
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IJwtTokenService _jwtTokenService;
    public AuthController(IAuthService authService, IJwtTokenService jwtTokenService)
    {
        _authService = authService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterRequestDto payload)
    {
      var userDto = await _authService.RegisterUser(payload.Name, payload.Email,  payload.Password, "User");
      return userDto;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto payload)
    {
      var loginResponse = await _authService.LoginUser(payload.Email, payload.Password);
      return loginResponse;
    }
}