using Application.Models.DTOs.Auth;
using Application.Models.DTOs.User;
using Domain.Models;

namespace Application.Services.IAuthServices;

public interface IAuthService
{
    Task<AuthTokenDto> LoginUser(LoginRequest request);
    Task<AuthTokenDto> RegisterUser(AddUserDto request);
    AuthTokenDto GenerateToken(AppUser user);
    Task<AuthTokenDto> RefreshToken(RefreshTokenDto request);
}