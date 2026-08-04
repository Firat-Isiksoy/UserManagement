using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserManagement.Models
{
    public class MovieModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public int? ReleaseYear { get; set; }
        public float AverageRating { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryModel Category { get; set; }
        public virtual ICollection<MovieRating> MovieRatings { get; set; } = new List<MovieRating>();
    }
}