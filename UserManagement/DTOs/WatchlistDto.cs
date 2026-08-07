namespace UserManagement.DTOs
{
    public class WatchlistDto
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public bool IsWatched { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime? WatchedAt { get; set; }
    }
}