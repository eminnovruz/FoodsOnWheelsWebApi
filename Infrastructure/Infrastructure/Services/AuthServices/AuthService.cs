using Application.Models.DTOs.AppUser;
using Application.Models.DTOs.Auth;
using Application.Models.DTOs.User;
using Application.Repositories;
using Application.Services.IAuthServices;
using Azure.Core;
using Domain.Models;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;

namespace Infrastructure.Services.AuthServices;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPassHashService _hashService;
    private readonly IValidator<AddAppUserDto> _addAppUserValidator;
    private readonly IJWTService _jwtService;

    public AuthService(IUnitOfWork unitOfWork, IPassHashService hashService, IValidator<AddAppUserDto> addAppUserValidator, IJWTService jwtService)
    {
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _addAppUserValidator = addAppUserValidator;
        _jwtService = jwtService;
    }


    public async Task<AuthTokenDto> LoginUser(LoginRequest request)
    {

        var user = await _unitOfWork.ReadUserRepository.GetAsync(req => req.Email == request.Email);
        if (user is not null)
        {
            if (!_hashService.ConfirmPasswordHash(request.Password, user.PassHash, user.PassSalt))
                throw new("Wrong password!");
            var token = GenerateToken(user);

            await _unitOfWork.WriteUserRepository.UpdateAsync(user.Id);
            await _unitOfWork.WriteUserRepository.SaveChangesAsync();
            return token;
        }        

        
        var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(req => req.Email == request.Email);
        if (restaurant is not null)
        {
            if (!_hashService.ConfirmPasswordHash(request.Password, restaurant.PassHash, restaurant.PassSalt))
                throw new("Wrong password!");
            var token = GenerateToken(restaurant);

            await _unitOfWork.WriteRestaurantRepository.UpdateAsync(restaurant.Id);
            await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();
            return token;
        }
        
        var worker = await _unitOfWork.ReadWorkerRepository.GetAsync(req => req.Email == request.Email);
        if (worker is not null)
        {
            if (!_hashService.ConfirmPasswordHash(request.Password, worker.PassHash, worker.PassSalt))
                throw new("Wrong password!");
            var token = GenerateToken(worker);

            await _unitOfWork.WriteWorkerRepository.UpdateAsync(worker.Id);
            await _unitOfWork.WriteWorkerRepository.SaveChangesAsync();
            return token;
        }
       
        var courier =await _unitOfWork.ReadCourierRepository.GetAsync(req => req.Email == request.Email);
        if (courier is not null)
        {
            if (!_hashService.ConfirmPasswordHash(request.Password, courier.PassHash, courier.PassSalt))
                throw new("Wrong password!");
            var token = GenerateToken(courier);

            await _unitOfWork.WriteCourierRepository.UpdateAsync(courier.Id);
            await _unitOfWork.WriteCourierRepository.SaveChangesAsync();
            return token;
        }
        throw new ArgumentNullException("You haven't an account!");
    }

    public async Task<AuthTokenDto> RegisterUser(AddUserDto request)
    {
        var isValid = _addAppUserValidator.Validate(request);

        if (isValid.IsValid)
        {
            var users = _unitOfWork.ReadUserRepository.GetAll().ToList();

            if (users.Count == 0)
            {
                var specUser = users.FirstOrDefault(c => c?.Email == request.Email);
                if (specUser is not null)
                    throw new ArgumentException("This email has already exsist!");
            }

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
                BankCardsId = new List<string>(),
                PhoneNumber = request.PhoneNumber,
                SelectBankCardId = "",
                Role = "User",
                RefreshToken = "",
                TokenExpireDate = default
            };



            var result = await _unitOfWork.WriteUserRepository.AddAsync(newUser);
            await _unitOfWork.WriteUserRepository.SaveChangesAsync();
            var token = GenerateToken(newUser);

            if (result)
               return token;
        }
        throw new ArgumentException("No Valid");

    }

    public AuthTokenDto GenerateToken(AppUser user)
    {
        var token = _jwtService.GenerateSecurityToken(user.Id, user.Email, user.Role);
        user.RefreshToken = token.RefreshToken;
        user.TokenExpireDate = token.ExpireDate;
        return token;
    }

    public async Task<AuthTokenDto> RefreshToken(RefreshTokenDto request)
    {
        if (request.ExpireDate >= DateTime.Now)
        {
            var user = await _unitOfWork.ReadUserRepository.GetAsync(req => req.RefreshToken == request.RefreshToken);
            if (user is not null)
            {
                var token = GenerateToken(user);
                await _unitOfWork.WriteUserRepository.UpdateAsync(user.Id);
                await _unitOfWork.WriteUserRepository.SaveChangesAsync();
                return token;
            }

            var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(req => req.RefreshToken == request.RefreshToken);
            if (restaurant is not null)
            {
                var token = GenerateToken(restaurant);
                await _unitOfWork.WriteRestaurantRepository.UpdateAsync(restaurant.Id);
                await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();
                return token;
            }

            var worker = await _unitOfWork.ReadWorkerRepository.GetAsync(req => req.RefreshToken == request.RefreshToken);
            if (worker is not null)
            {
                var token = GenerateToken(worker);
                await _unitOfWork.WriteWorkerRepository.UpdateAsync(worker.Id);
                await _unitOfWork.WriteWorkerRepository.SaveChangesAsync();
                return token;
            }

            var courier = await _unitOfWork.ReadCourierRepository.GetAsync(req => req.RefreshToken == request.RefreshToken);
            if (courier is not null)
            {
                var token = GenerateToken(courier);
                await _unitOfWork.WriteCourierRepository.UpdateAsync(courier.Id);
                await _unitOfWork.WriteCourierRepository.SaveChangesAsync();
                return token;
            }
        }

        throw new ArgumentException();
    }
}
