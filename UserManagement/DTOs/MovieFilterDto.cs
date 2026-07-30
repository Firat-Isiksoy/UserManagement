namespace UserManagement.DTOs
{
    public class MovieFilterDto
    {
        public Guid? CategoryId { get; set; }
        public string? SortBy { get; set; }  
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
