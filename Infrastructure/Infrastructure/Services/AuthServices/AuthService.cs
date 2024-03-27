using Application.Models.DTOs.AppUser;
using Application.Models.DTOs.Auth;
using Application.Models.DTOs.User;
using Application.Repositories;
using Application.Services.IAuthServices;
using Domain.Models;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

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

    public string LoginUser(LoginRequest request)
    {
        var users = _unitOfWork.ReadUserRepository.GetAll();
        if (users is not null)
        {
            var user = users.FirstOrDefault(req => req?.Email == request.Email,null);
            if (user is not null)
            {
                if (!_hashService.ConfirmPasswordHash(request.Password, user.PassHash, user.PassSalt))
                    throw new("Wrong password!");
                return _jwtService.GenerateSecurityToken(user.Id, user.Email);
            }
        }

        var restaurants = _unitOfWork.ReadRestaurantRepository.GetAll();
        if (restaurants is not null)
        {
            var restaurant = restaurants.FirstOrDefault(req => req?.Email == request.Email,null);
            if (restaurant is not null)
            {
                if (!_hashService.ConfirmPasswordHash(request.Password, restaurant.PassHash, restaurant.PassSalt))
                    throw new("Wrong password!");
                return _jwtService.GenerateSecurityToken(restaurant.Id, restaurant.Email);
            }
        }

        var workers = _unitOfWork.ReadWorkerRepository.GetAll();
        if (workers is not null)
        {
            var worker = workers.FirstOrDefault(req => req?.Email == request.Email, null);
            if (worker is not null)
            {
                if (!_hashService.ConfirmPasswordHash(request.Password, worker.PassHash, worker.PassSalt))
                    throw new("Wrong password!");
                return _jwtService.GenerateSecurityToken(worker.Id, worker.Email);
            }
        }

        var couriers = _unitOfWork.ReadCourierRepository.GetAll();
        if (couriers is not null)
        {
            var courier = couriers.FirstOrDefault(req => req?.Email == request.Email, null);
            if (courier is not null)
            {
                if (!_hashService.ConfirmPasswordHash(request.Password, courier.PassHash, courier.PassSalt))
                    throw new("Wrong password!");
                return _jwtService.GenerateSecurityToken(courier.Id, courier.Email);
            }
        }

        throw new ArgumentNullException("You haven't an account!");
    }

    public async Task<bool> RegisterUser(AddUserDto request)
    {
        var isValid = _addAppUserValidator.Validate(request);

        if (isValid.IsValid)
        {

            var users = _unitOfWork.ReadUserRepository.GetAll();

            var specUser = users.FirstOrDefault(c => c.Email == request.Email);
            if (specUser is not null)
                throw new ArgumentException("This email has already exsist!");

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
                PhoneNumber = request.PhoneNumber
            };

            var result = await _unitOfWork.WriteUserRepository.AddAsync(newUser);
            await _unitOfWork.WriteUserRepository.SaveChangesAsync();
            return result;
        }
        throw new ArgumentException("No Valid");

    }
}
