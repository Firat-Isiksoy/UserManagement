namespace UserManagement.DTOs
{
    public class MovieDto
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        public float AverageRating { get; set; }
        public int? ReleaseYear { get; set; }
        public string? Description { get; set; }

    }
}