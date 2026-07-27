using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class CategoryModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<MovieModel> Movies { get; set; } = new List<MovieModel>();
    }
}
