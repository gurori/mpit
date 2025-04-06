using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace mpit.mpit.Application.Interfaces.Auth;

public interface IJwtProvider
{
    public Task<string> GenerateTokenAsync(Guid userId, string role);
    public ClaimsPrincipal ValidateToken(string token);
    public Task<TokenValidationResult> ValidateTokenAsync(string token);
}
