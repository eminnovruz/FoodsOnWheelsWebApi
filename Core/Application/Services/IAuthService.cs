using Application.Models.DTOs.Auth;

namespace Application.Services;

public interface IAuthService
{
    string LoginUser(UserLoginRequest request);
    Task<bool> RegisterUser(UserRegisterRequest request);
}
