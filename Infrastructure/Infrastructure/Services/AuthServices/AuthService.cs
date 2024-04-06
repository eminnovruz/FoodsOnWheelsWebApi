using Application.Models.DTOs.AppUser;
using Application.Models.DTOs.Auth;
using Application.Models.DTOs.User;
using Application.Repositories;
using Application.Services.IAuthServices;
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
        var users = _unitOfWork.ReadUserRepository.GetAll().ToList();
        if (users.Count != 0)
        {
            var user = users.FirstOrDefault(req => req?.Email == request.Email, null);
            if (user is not null)
            {
                var token = GenerateToken(user, request.Password);
                user.TokenExpireDate = token.ExpireDate;
                user.RefreshToken = token.RefreshToken;

                await _unitOfWork.WriteUserRepository.UpdateAsync(user.Id);
                await _unitOfWork.WriteUserRepository.SaveChangesAsync();
                return token;
            }
        }

        var restaurants = _unitOfWork.ReadRestaurantRepository.GetAll().ToList();
        if (restaurants.Count != 0)
        {
            var restaurant = restaurants.FirstOrDefault(req => req?.Email == request.Email, null);
            if (restaurant is not null)
            {
                var token = GenerateToken(restaurant, request.Password);
                restaurant.TokenExpireDate = token.ExpireDate;
                restaurant.RefreshToken = token.RefreshToken;

                await _unitOfWork.WriteRestaurantRepository.UpdateAsync(restaurant.Id);
                await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();
                return token;
            }
        }

        var workers = _unitOfWork.ReadWorkerRepository.GetAll().ToList();
        if (workers.Count == 0)
        {
            var worker = workers.FirstOrDefault(req => req?.Email == request.Email, null);
            if (worker is not null)
                if (worker is not null)
                {
                    var token = GenerateToken(worker, request.Password);
                    worker.TokenExpireDate = token.ExpireDate;
                    worker.RefreshToken = token.RefreshToken;

                    await _unitOfWork.WriteWorkerRepository.UpdateAsync(worker.Id);
                    await _unitOfWork.WriteWorkerRepository.SaveChangesAsync();
                    return token;
                }
        }

        var couriers = _unitOfWork.ReadCourierRepository.GetAll().ToList();
        if (couriers.Count == 0)
        {
            var courier = couriers.FirstOrDefault(req => req?.Email == request.Email, null);
            if (courier is not null)
            {

                var token = GenerateToken(courier, request.Password);
                courier.TokenExpireDate = token.ExpireDate;
                courier.RefreshToken = token.RefreshToken;

                await _unitOfWork.WriteCourierRepository.UpdateAsync(courier.Id);
                await _unitOfWork.WriteCourierRepository.SaveChangesAsync();
                return token;
            }
        }

        throw new ArgumentNullException("You haven't an account!");
    }

    public async Task<bool> RegisterUser(AddUserDto request)
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

            // var token = GenerateToken(newUser, request.Password);


            var result = await _unitOfWork.WriteUserRepository.AddAsync(newUser);
            await _unitOfWork.WriteUserRepository.SaveChangesAsync();
            return result;
        }
        throw new ArgumentException("No Valid");

    }

    public AuthTokenDto GenerateToken(AppUser user, string password)
    {
        if (!_hashService.ConfirmPasswordHash(password, user.PassHash, user.PassSalt))
            throw new("Wrong password!");
        var token = _jwtService.GenerateSecurityToken(user.Id, user.Email, user.Role);
        user.RefreshToken = token.RefreshToken;
        user.TokenExpireDate = token.ExpireDate;
        return token;
    }
}
