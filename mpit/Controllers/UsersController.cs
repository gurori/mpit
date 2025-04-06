using Microsoft.AspNetCore.Mvc;
using mpit.mpit.Application.Services;
using mpit.mpit.Core.DTOs.User;

namespace mpit.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class UsersController(UsersService usersService) : BaseController
{
    private readonly UsersService _usersService = usersService;

    [HttpPost("register")]
    public async Task<IActionResult> Register() =>
        await TryCatchAsync(async () =>
        {
            await Task.CompletedTask;
            return Ok();
        });

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request) =>
        await TryCatchAsync(async () =>
        {
            string token = await _usersService.LoginAsync(
                request.Login,
                request.Password,
                request.Role
            );
            return Ok(token);
        });
}
