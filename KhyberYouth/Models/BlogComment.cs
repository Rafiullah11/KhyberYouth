using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.Models
{
    public class BlogComment
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, StringLength(1000)]
        public string Text { get; set; }
        public DateTime PostedOn { get; set; }
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
