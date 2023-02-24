using System.Security.Claims;

namespace GGooseBlog.Services;

public interface ITokenService
{
    string GenerateJwtToken();
    string GenerateRefreshToken();
    ClaimsPrincipal GetClaimsPrincipalFromToken(string jwtToken);
}