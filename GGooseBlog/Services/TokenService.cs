using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GGooseBlog.Services;

public class TokenService : ITokenService
{
    public string GenerateJwtToken()
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY") ?? throw new InvalidOperationException()));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: "https://localhost:7070",
            audience: "https://localhost:7070",
            expires: DateTime.Now.AddMinutes(2),
            signingCredentials: signingCredentials
        );
        
        var jwtTokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return jwtTokenString;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetClaimsPrincipalFromToken(string jwtToken)
    {
        var jwtTokenValidationParams = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                Environment.GetEnvironmentVariable("SECRET_KEY") ?? throw new InvalidOperationException()))
        };
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var claimsPrincipal = jwtTokenHandler.ValidateToken(jwtToken, jwtTokenValidationParams, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken 
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
            throw new SecurityTokenException();

        return claimsPrincipal;
    }
}