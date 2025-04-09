using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mpit.mpit.Application.Interfaces.Services;
using mpit.mpit.Core.DTOs.User;

namespace mpit.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class UsersController(IUsersService usersService) : BaseController
{
    private readonly IUsersService _usersService = usersService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request) =>
        await TryCatchAsync(async () =>
        {
            await _usersService.RegisterAsync(request.Login, request.Password, request.Role);
            return Ok();
        });

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request) =>
        await TryCatchAsync(async () =>
        {
            string token = await _usersService.LoginAsync(request.Login, request.Password);
            return Ok(token);
        });

    [HttpPost("info")]
    [Authorize]
    public async Task<IActionResult> AddInfo(InfoRequest request) =>
        await TryCatchAsync(async () =>
        {
            string token = GetTokenFromHeaders();
            await _usersService.AddInfoAsync(token, request);
            return Ok();
        });
}
