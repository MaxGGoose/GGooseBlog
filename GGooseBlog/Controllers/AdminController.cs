using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GGooseBlog.Controllers;

//[Authorize]
public class AdminController : Controller
{
    [Route("admin")]
    public IActionResult Admin() => File("html/edit-post-list.html", "text/html");
    
    [Route("create")]
    public IActionResult Create() => File("html/create-post.html", "text/html");
    
    [Route("edit")]
    public IActionResult Edit() => File("html/create-post.html", "text/html");
}