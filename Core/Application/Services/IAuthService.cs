using Application.Models.DTOs.Auth;

namespace Application.Services;

public interface IAuthService
{
    Task<string> LoginUser(UserLoginRequest request);
    Task<bool> RegisterUser(UserRegisterRequest request);
}
