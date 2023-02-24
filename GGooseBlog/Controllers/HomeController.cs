using Microsoft.AspNetCore.Mvc;

namespace GGooseBlog.Controllers;

public class HomeController : Controller
{
    [Route("all")]
    public IActionResult All() => File("html/all-posts.html", "text/html");
    [Route("post")]
    public IActionResult Post() => File("html/current-post-by-id.html", "text/html");
    [Route("auth")]
    public IActionResult Auth() => File("html/auth.html", "text/html");
}