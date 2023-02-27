using Microsoft.AspNetCore.Mvc;

namespace GGooseBlog.Controllers;

public class HomeController : Controller
{
    [Route("auth")]
    public IActionResult Auth() => File("html/auth.html", "text/html");
}