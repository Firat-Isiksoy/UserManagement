using UserManagement.Models;
using UserManagement.DTOs;

namespace UserManagement.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(this UserModel user)
        {
            if (user == null) return null;
            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }
        public static UserDetailsDto ToDetailsDto(this UserModel user)
        {
            if (user == null) return null;
            return new UserDetailsDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role
            };
        }
        public static UserModel ToModel(this UserCreateDto dto)
        {
            if (dto == null) return null;
            return new UserModel
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
                Role = "User"
            };
        }
        public static void UpdateModel(this UserCreateDto dto, UserModel model)
        {
            if (dto == null || model == null) return;

            model.FirstName = dto.FirstName;
            model.LastName = dto.LastName;
            model.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password))
            {
                model.Password = dto.Password;
            }
        }
    }
}