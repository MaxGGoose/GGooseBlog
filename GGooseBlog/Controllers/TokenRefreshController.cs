using GGooseBlog.Models;
using GGooseBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GGooseBlog.Controllers;

[Route("api/auth")]
public class TokenRefreshController : Controller
{
    private ITokenService TokenService { get; }
    private ApplicationContext Db { get; }

    public TokenRefreshController(ITokenService tokenService, ApplicationContext db)
    {
        TokenService = tokenService;
        Db = db;
    }

    [HttpPut("refresh")]
    public async Task<IActionResult> Refresh([FromBody]AuthApiModel? authApiModel)
    {
        if (authApiModel is null 
            || string.IsNullOrEmpty(authApiModel.JwtToken) 
            || string.IsNullOrEmpty(authApiModel.RefreshToken)) 
            return BadRequest("Invalid request");

        var refreshToken = authApiModel.RefreshToken;

        var loginModel = await Db.LoginModels.SingleOrDefaultAsync(model => model.RefreshToken == refreshToken);

        if (loginModel is null)
        {
            return BadRequest("Invalid request");
        }
        if (loginModel.RefreshTokenExpirationTime <= DateTime.Now)
        {
            Db.LoginModels.Remove(loginModel);
            return BadRequest("Invalid request");
        }

        var newJwtToken = TokenService.GenerateJwtToken();
        var newRefreshToken = TokenService.GenerateRefreshToken();

        loginModel.RefreshToken = newRefreshToken;

        await Db.SaveChangesAsync();
        
        return Ok(new AuthApiModel
        {
            JwtToken = newJwtToken,
            RefreshToken = newRefreshToken
        });
    }
}