using Application.Models.DTOs.Auth;
using Application.Models.DTOs.User;

namespace Application.Services.IAuthServices;

public interface IAuthService
{
    AuthTokenDto LoginUser(LoginRequest request);
    Task<bool> RegisterUser(AddUserDto request);
}