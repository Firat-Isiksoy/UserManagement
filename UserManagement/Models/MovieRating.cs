using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class MovieRating
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
        public float Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Note { get; set; }
    }
}
