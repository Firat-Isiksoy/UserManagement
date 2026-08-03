using UserManagement.Models;

public class MovieDetailsDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public int ReleaseYear { get; set; }
    public float AverageRating { get; set; }
    public Guid CategoryId { get; set; }
    public PagedResponse<RatingDetailsDto> Ratings { get; set; }
}