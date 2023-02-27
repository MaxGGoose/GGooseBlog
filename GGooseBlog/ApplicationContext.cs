using System.Diagnostics.CodeAnalysis;
using GGooseBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace GGooseBlog;

public sealed class ApplicationContext : DbContext
{
    public DbSet<BlogPostModel> BlogPosts { get; set; } = null!;
    public DbSet<LoginModel> LoginModels { get; set; } = null!;

    [SuppressMessage("ReSharper.DPA", "DPA0006: Large number of DB commands", MessageId = "count: 646")]
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