using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class UserModel
    {
        [Key] 
        public Guid Id { get; set; }
        [Required] 
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; } = "123456";
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
