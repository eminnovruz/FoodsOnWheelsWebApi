using Application.Models.DTOs.Worker;
using Application.Repositories;
using Application.Repositories.CategoryRepository;
using Application.Repositories.CourierRepository;
using Application.Repositories.FoodRepository;
using Application.Repositories.OrderRepository;
using Application.Repositories.RestaurantRepository;
using Application.Repositories.UserRepository;
using Application.Services;
using Domain.Models;
using FluentValidation;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repositories;
using Persistence.Repositories.CategoryRepository;
using Persistence.Repositories.CourierRepository;
using Persistence.Repositories.FoodRepository;
using Persistence.Repositories.OrderRepository;
using Persistence.Repositories.RestaurantRepository;
using Persistence.Repositories.UserRepository;
using Persistence.Services;
using WebApi.Validators;

public static class Extension
{

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICourierService, CourierService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWorkerService, WorkerService>();

        services.AddValidatorsFromAssemblyContaining<AddRestaurantDtoValidator>();
        services.AddTransient<IValidator<AddRestaurantDto>, AddRestaurantDtoValidator>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IReadCourierRepository, ReadCourierRepository>();
        services.AddScoped<IWriteCourierRepository, WriteCourierRepository>();

        services.AddScoped<IWriteRestaurantRepository, WriteRestaurantRepository>();
        services.AddScoped<IReadRestaurantRepository, ReadRestaurantRepository>();

        services.AddScoped<IWriteUserRepository, WriteUserRepository>();
        services.AddScoped<IReadUserRepository, ReadUserRepository>();

        services.AddScoped<IReadOrderRepository, ReadOrderRepository>();
        services.AddScoped<IWriteOrderRepository, WriteOrderRepository>();

        services.AddScoped<IReadFoodRepository, ReadFoodRepository>();
        services.AddScoped<IWriteFoodRepository, WriteFoodRepository>();

        services.AddScoped<IReadCategoryRepository, ReadCategoryRepository>();
        services.AddScoped<IWriteCategoryRepository, WriteCategoryRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}