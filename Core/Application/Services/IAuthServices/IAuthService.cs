using Application.Models.DTOs.Auth;

namespace Application.Services.IAuthServices;

public interface IAuthService
{
    string LoginUser(UserLoginRequest request);
    Task<bool> RegisterUser(UserRegisterRequest request);
}