using UserManagement.Models;

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
                Email = user.Email,
            };
        }
        public static UserModel ToModel(this UserDto dto)
        {
            if (dto == null) return null;
            return new UserModel
            {
                FirstName =dto.FirstName,
                LastName = dto.LastName,   
                Email = dto.Email,
            };
        }
        public static void UpdateModel(this UserDto dto, UserModel model)
        {
            if (dto == null || model == null) return;
            model.FirstName = dto.FirstName;
            model.LastName = dto.LastName;
            model.Email = dto.Email;
        }
    }
}
