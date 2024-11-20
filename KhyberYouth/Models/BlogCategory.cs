using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.Models
{
    public class BlogCategory
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
