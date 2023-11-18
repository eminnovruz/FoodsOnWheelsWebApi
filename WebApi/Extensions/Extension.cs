using Application.Models;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Context;
using Persistence.Repositories;
using Persistence.Repositories.CategoryRepository;
using Persistence.Repositories.CourierRepository;
using Persistence.Repositories.FoodRepository;
using Persistence.Repositories.OrderRepository;
using Persistence.Repositories.RestaurantRepository;
using Persistence.Repositories.UserRepository;
using Persistence.Services;
using System.Text;
using WebApi.Validators;

public static class Extension
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "My Api - V1",
                    Version = "v1",
                }
            );

            setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Jwt Authorization header using the Bearer scheme/ \r\r\r\n Enter 'Bearer' [space] and then token in the text input below. \r\n\r\n Example : \"Bearer f3c04qc08mh3n878\""
            });

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id ="Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
        });
        return services;
    }

    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJWTService, JWTService>();

        var jwtConfig = new JWTConfiguration();
        configuration.GetSection("JWT").Bind(jwtConfig);

        services.AddSingleton(jwtConfig);


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, setup =>
        {
            setup.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = jwtConfig.Audience,
                ValidIssuer = jwtConfig.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
            };
        });

        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICourierService, CourierService>();
        services.AddScoped<IWorkerService, WorkerService>();
        services.AddScoped<IPassHashService, PassHashService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBlobService, BlobService>();

        services.AddValidatorsFromAssemblyContaining<AddRestaurantDtoValidator>();
        services.AddTransient<IValidator<AddRestaurantDto>, AddRestaurantDtoValidator>();
        services.AddTransient<IValidator<AddCourierDto>, AddCourierDtoValidator>();

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