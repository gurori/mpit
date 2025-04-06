using Microsoft.AspNetCore.Mvc;
using mpit.mpit.Core.Exceptions;

namespace mpit.Controllers;

public abstract class BaseController : ControllerBase
{
    protected async Task<IActionResult> TryCatchAsync(Func<Task<IActionResult>> func)
    {
        try
        {
            return await func();
        }
        catch (ApiException e)
        {
            return Problem(detail: e.Message, statusCode: e.StatusCode);
        }
    }

    protected string GetTokenFromHeaders() =>
        Request.Headers.Authorization.FirstOrDefault()!.Split(" ").Last();
}
