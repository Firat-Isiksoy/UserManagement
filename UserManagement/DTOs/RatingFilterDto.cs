using UserManagement.DTOs;
public class RatingFilterDto : PaginationFilter
{
    public Guid? MovieId { get; set; }
    public Guid? UserId { get; set; }
    public string? SortBy { get; set; }
}