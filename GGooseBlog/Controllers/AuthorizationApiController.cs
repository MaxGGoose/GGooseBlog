using GGooseBlog.Services;
using GGooseBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GGooseBlog.Controllers;

[Route("api/auth")]
public class AuthorizationApiController : Controller
{
    private ITokenService TokenService { get; }
    private ApplicationContext Db { get; }
    
    public AuthorizationApiController(ITokenService tokenService, ApplicationContext db)
    {
        TokenService = tokenService;
        Db = db;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]string? accessCode)
    {
        if (User.Identity is not null && User.Identity.IsAuthenticated)
            Response.Redirect("/");
        
        if (string.IsNullOrEmpty(accessCode)) return BadRequest("Invalid request");
        if (accessCode != Environment.GetEnvironmentVariable("ACCESS_CODE")) return Unauthorized("Invalid access code");

        var jwtToken = TokenService.GenerateJwtToken();
        var refreshToken = TokenService.GenerateRefreshToken();
        
        var loginModel = new LoginModel
        {
            RefreshToken = refreshToken,
            RefreshTokenExpirationTime = DateTime.Now.AddDays(7)
        };

        await Db.LoginModels.AddAsync(loginModel);
        await Db.SaveChangesAsync();
        
        return Ok(new AuthApiModel
        {
            JwtToken = jwtToken,
            RefreshToken = refreshToken
        });
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody]AuthApiModel authApiModel)
    {
        var loginModel = await Db.LoginModels.SingleOrDefaultAsync(model => model.RefreshToken == authApiModel.RefreshToken);

        if (loginModel is null)
            return BadRequest("Invalid request");

        Db.LoginModels.Remove(loginModel);
        await Db.SaveChangesAsync();

        return Ok(authApiModel);
    }
}