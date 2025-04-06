using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using mpit.mpit.Application.Interfaces.Auth;
using mpit.mpit.Infastructure.Auth;

namespace mpit.mpit.Infastructure.Auth
{
    public class JwtProvider(IOptions<JwtOptions> options, IPermissionService permissionService)
        : IJwtProvider
    {
        private readonly IPermissionService _permissionService = permissionService;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();
        private readonly JwtOptions _options = options.Value;

        public async Task<string> GenerateTokenAsync(Guid userId, string role)
        {
            List<Claim> claims = [new(CustomClaims.UserId, userId.ToString())];

            HashSet<string> permissions = await _permissionService.GetPermissionsAsync(role);

            foreach (string permission in permissions)
                claims.Add(new(CustomClaims.Permissions, permission));

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddDays(_options.ExpiresDays)
            );

            return _tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenValidationParameters = JwtParameters.GetTokenValidationParameters(_options);

            return _tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
        }

        public async Task<TokenValidationResult> ValidateTokenAsync(string token)
        {
            var tokenValidationParameters = JwtParameters.GetTokenValidationParameters(_options);

            return await _tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
        }
    }
}
