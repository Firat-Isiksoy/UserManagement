using UserManagement.Models;

namespace UserManagement.DTOs
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public List<MovieModel> Movies { get; set; }
    }
}