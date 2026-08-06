using Azure.Core; 
using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Services
{
    public interface IAuthService
    {
        ResponseModel<string> Login(LoginDto request);
        ResponseModel<UserDetailsDto> Register(UserCreateDto request);
        ResponseModel<UserDetailsDto> UpdateProfile(Guid userId, UserCreateDto request);
        bool DeleteAccount(Guid userId);
    }
}