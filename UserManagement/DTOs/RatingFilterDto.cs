public class RatingFilterDto
{
    public Guid? MovieId { get; set; }
    public Guid? UserId { get; set; }
    public string? SortBy { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}