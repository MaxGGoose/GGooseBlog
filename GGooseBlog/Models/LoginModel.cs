namespace GGooseBlog.Models;

public class LoginModel
{
    public int Id { get; set; }
    public string RefreshToken { get; set; } = "";
    public DateTime RefreshTokenExpirationTime { get; set; }
}