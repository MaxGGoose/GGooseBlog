using GGooseBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GGooseBlog.Controllers;

[Authorize]
[Route("api/blogpost")]
public class BlogPostApiController : Controller
{
    private ILogger<BlogPostApiController> Logger { get; }
    private ApplicationContext Db { get; }

    public BlogPostApiController(ILogger<BlogPostApiController> logger, ApplicationContext db)
    {
        Logger = logger;
        Db = db;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateBlogPost(BlogPostModel blogPost)
    {
        if (blogPost.Title != string.Empty && blogPost.Text != string.Empty)
        {
            blogPost.Created = DateTime.Now.ToString("O");
            blogPost.LastEdited = blogPost.Created;

            await Db.BlogPosts.AddAsync(blogPost);
            await Db.SaveChangesAsync();
            
            Logger.LogInformation("User successfully sent request to /api/blogPost post endpoint");

            return Ok(blogPost);
        } 
        Logger.LogInformation("User sent incorrect data to /api/blogPost post endpoint");
        return BadRequest("Invalid request");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBlogPosts()
    {
        Logger.LogInformation("User got all existent objects from /api/blogPost get endpoint");
        return Ok(await Db.BlogPosts.ToArrayAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBlogPost(int id)
    {
        var blogPost = await Db.BlogPosts.FirstOrDefaultAsync(blogPost => blogPost.Id == id);

        if (blogPost is not null)
        {
            Logger.LogInformation("User got object with id {postId} from /api/blogPost get endpoint", id);
            return Ok(blogPost);
        }
        Logger.LogInformation("User tried to access nonexistent object at /api/blogPost get endpoint");
        return NotFound("Blog post not found");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBlogPost(BlogPostModel newBlogPost, int id)
    {
        var oldBlogPost = await Db.BlogPosts.FirstOrDefaultAsync(blogPost => blogPost.Id == id);
        if (oldBlogPost is not null && newBlogPost.Title != string.Empty && newBlogPost.Text != string.Empty)
        {
            oldBlogPost.Title = newBlogPost.Title;
            oldBlogPost.Text = newBlogPost.Text;
            oldBlogPost.LastEdited = DateTime.Now.ToString("O");

            await Db.SaveChangesAsync();
        
            Logger.LogInformation("User changed object with {id} id with /api/blogPost put endpoint", id);
            return Ok(oldBlogPost);
        }
        Logger.LogInformation("User sent incorrect data to /api/blogPost put endpoint");
        return BadRequest("Invalid request");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBlogPost(int id)
    {
        var blogPost = await Db.BlogPosts.FirstOrDefaultAsync(blogPost => blogPost.Id == id);

        if (blogPost is not null)
        {
            Db.BlogPosts.Remove(blogPost);

            await Db.SaveChangesAsync();

            Logger.LogInformation("User deleted object with {id} id with /api/blogPost delete endpoint", id);
            return Ok(blogPost);
        }

        Logger.LogInformation("User tried to access nonexistent object at /api/blogPost delete endpoint");
        return NotFound("Blog post not found");
    }
}