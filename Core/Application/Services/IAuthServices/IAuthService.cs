using Application.Models.DTOs.Auth;

namespace Application.Services.IAuthServices;

public interface IAuthService
{
    string LoginUser(LoginRequest request);
    Task<bool> RegisterUser(UserRegisterRequest request);
}