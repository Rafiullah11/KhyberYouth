using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}