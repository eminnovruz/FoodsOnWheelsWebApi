using Application.Models.DTOs.Auth;
using Application.Repositories;
using Application.Services;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<string> LoginUser(UserLoginRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RegisterUser(UserRegisterRequest request)
    {
        throw new NotImplementedException();
    }
}
