using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.Models
{
    public class BlogAuthor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public ICollection<BlogPost>? BlogPosts { get; set; }
    }
}
