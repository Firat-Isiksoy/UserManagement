using System.ComponentModel.DataAnnotations;

namespace UserManagement.DTOs
{
    public class MovieRatingDto
    {
        [Required]
        public Guid MovieId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [Range(1,10)]
        public int Rating { get; set; }
        public string? Note { get; set; }
    }
}
