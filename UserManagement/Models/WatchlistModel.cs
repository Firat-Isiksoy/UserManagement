using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models
{
    public class WatchlistModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserModel? User { get; set; }

        [Required]
        public Guid MovieId { get; set; }

        [ForeignKey(nameof(MovieId))]
        public MovieModel? Movie { get; set; }

        public bool IsWatched { get; set; } = false;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public DateTime? WatchedAt { get; set; }
    }
}