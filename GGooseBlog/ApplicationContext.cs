using GGooseBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace GGooseBlog;

public sealed class ApplicationContext : DbContext
{
    public DbSet<BlogPostModel> BlogPosts { get; set; } = null!;
    public DbSet<LoginModel> LoginModels { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite();
    }
}