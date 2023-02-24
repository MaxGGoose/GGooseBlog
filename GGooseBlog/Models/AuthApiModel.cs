namespace GGooseBlog.Models;

public class AuthApiModel
{
    public string JwtToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}