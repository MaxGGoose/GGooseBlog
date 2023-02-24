using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GGooseBlog.Models;

public class BlogPostModel
{
    [Key]
    public int Id { get; set; }
    
    [MinLength(50)]
    public string Title { get; set; } = "";
    
    [MinLength(50)]
    public string Text { get; set; } = "";
    
    public string Created { get; set; } = "";
    public string LastEdited { get; set; } = "";
}