using Application.Models.DTOs.Auth;
using Application.Repositories;
using Application.Services.IAuthServices;
using Domain.Models;

namespace Infrastructure.Services.AuthServices;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPassHashService _hashService;
    private readonly IJWTService _jwtService;

    public AuthService(IUnitOfWork unitOfWork, IPassHashService hashService, IJWTService jwtService)
    {
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _jwtService = jwtService;
    }

    public string LoginUser(UserLoginRequest request)
    {
        var users = _unitOfWork.ReadUserRepository.GetAll();
        
        var specUser = users.FirstOrDefault(c => c.Email == request.Email);
        if (!_hashService.ConfirmPasswordHash(request.Password, specUser.PassHash, specUser.PassSalt))
            throw new("Wrong password!");
        
        return _jwtService.GenerateSecurityToken(specUser.Id, specUser.Email);
    }

    public async Task<bool> RegisterUser(UserRegisterRequest request)
    {
        var users = _unitOfWork.ReadUserRepository.GetAll();

        var specUser = users.FirstOrDefault(c => c.Email == request.Email);
        if (specUser is not null)
            throw new("This email has already exsist!");
        
        _hashService.Create(request.Password, out byte[] passHash, out byte[] passSalt);
       
        var newUser = new User()
        {
            Name = request.Name,
            Surname = request.Surname,
            PassHash = passHash,
            PassSalt = passSalt,
            BirthDate = request.BirthDate,
            Email = request.Email,
            Id = Guid.NewGuid().ToString(),
            OrderIds = new List<string>(),
            PhoneNumber = request.PhoneNumber
        };

        var result = await _unitOfWork.WriteUserRepository.AddAsync(newUser);
        await _unitOfWork.WriteUserRepository.SaveChangesAsync();
        return result;
    }
}
