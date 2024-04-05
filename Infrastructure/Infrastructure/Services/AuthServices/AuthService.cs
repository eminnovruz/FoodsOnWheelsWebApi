﻿using Application.Models.DTOs.AppUser;
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
        var users = _unitOfWork.ReadUserRepository.GetAll().ToList();
        if (users.Count != 0)
        {
            var user = users.FirstOrDefault(req => req?.Email == request.Email,null);
            if (user is not null)
            {
                if (!_hashService.ConfirmPasswordHash(request.Password, user.PassHash, user.PassSalt))
                    throw new("Wrong password!");
                return _jwtService.GenerateSecurityToken(user.Id, user.Email , user.Role);
            }
        }

        var restaurants = _unitOfWork.ReadRestaurantRepository.GetAll().ToList();
        if (restaurants.Count != 0)
        {
            var restaurant = restaurants.FirstOrDefault(req => req?.Email == request.Email, null);
            if (restaurant is not null)
            {
                if (!_hashService.ConfirmPasswordHash(request.Password, restaurant.PassHash, restaurant.PassSalt))
                    throw new("Wrong password!");
                return _jwtService.GenerateSecurityToken(restaurant.Id, restaurant.Email, "Restaurant");
            }
        }

        var workers = _unitOfWork.ReadWorkerRepository.GetAll().ToList();
        if (workers.Count == 0)
        {
            var worker = workers.FirstOrDefault(req => req?.Email == request.Email, null);
            if (worker is not null)
            {
                if (!_hashService.ConfirmPasswordHash(request.Password, worker.PassHash, worker.PassSalt))
                    throw new("Wrong password!");
                return _jwtService.GenerateSecurityToken(worker.Id, worker.Email, "Worker");
            }
        }

        var couriers = _unitOfWork.ReadCourierRepository.GetAll().ToList();
        if (couriers.Count == 0)
        {
            var courier = couriers.FirstOrDefault(req => req?.Email == request.Email, null);
            if (courier is not null)
            {
                if (!_hashService.ConfirmPasswordHash(request.Password, courier.PassHash, courier.PassSalt))
                    throw new("Wrong password!");
                return _jwtService.GenerateSecurityToken(courier.Id, courier.Email, "Courier");
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
                Role = "User"
            };

            var result = await _unitOfWork.WriteUserRepository.AddAsync(newUser);
            await _unitOfWork.WriteUserRepository.SaveChangesAsync();
            return result;
        }
        throw new ArgumentException("No Valid");

    }
}
