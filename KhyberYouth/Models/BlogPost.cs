using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        public string? FeaturedImage { get; set; }
        public DateTime PublishedOn { get; set; } = DateTime.UtcNow;
        public int AuthorId { get; set; }
        public BlogAuthor BlogAuthor { get; set; }
        public int CategoryId { get; set; }
        public BlogCategory BlogCategory { get; set; }
        public ICollection<BlogComment> BlogComments { get; set; }
    }
}
