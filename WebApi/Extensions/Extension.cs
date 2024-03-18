using Application.Models.Config;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Restaurant;
using Application.Repositories;
using Application.Repositories.CategoryRepository;
using Application.Repositories.CourierCommentRepository;
using Application.Repositories.CourierRepository;
using Application.Repositories.FoodRepository;
using Application.Repositories.OrderRatingRepository;
using Application.Repositories.OrderRepository;
using Application.Repositories.RestaurantCommentRepository;
using Application.Repositories.RestaurantRepository;
using Application.Repositories.UserRepository;
using Application.Repositories.WorkerRepository;
using Application.Services.IAuthServices;
using Application.Services.IHelperServices;
using Application.Services.IUserServices;
using FluentValidation;
using Infrastructure.Services.AuthServices;
using Infrastructure.Services.HelperServices;
using Infrastructure.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Repositories;
using Persistence.Repositories.CategoryRepository;
using Persistence.Repositories.CourierCommentRepository;
using Persistence.Repositories.CourierRepository;
using Persistence.Repositories.FoodRepository;
using Persistence.Repositories.OrderRatingRepository;
using Persistence.Repositories.OrderRepository;
using Persistence.Repositories.RestaurantCommentRepository;
using Persistence.Repositories.RestaurantRepository;
using Persistence.Repositories.UserRepository;
using Persistence.Repositories.WorkerRepository;
using System.Configuration;
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

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICourierService, CourierService>();
        services.AddScoped<IWorkerService, WorkerService>();
        services.AddScoped<IPassHashService, PassHashService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBlobService, BlobService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRestaurantService, RestaurantService>();
        services.AddScoped<IMailService, MailService>();

        services.AddValidatorsFromAssemblyContaining<AddRestaurantDtoValidator>();
        services.AddTransient<IValidator<AddRestaurantDto>, AddRestaurantDtoValidator>();
        services.AddTransient<IValidator<AddCourierDto>, AddCourierDtoValidator>();

        var smtpConfig = new SMTPConfig();
        configuration.GetSection("SMTP").Bind(smtpConfig);
        services.AddSingleton(smtpConfig);

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
        
        services.AddScoped<IReadCourierCommentRepository, ReadCourierCommentRepository>();
        services.AddScoped<IWriteCourierCommentRepository, WriteCourierCommentRepository>();
        
        services.AddScoped<IReadRestaurantCommentRepository, ReadRestaurantCommentRepository>();
        services.AddScoped<IWriteRestaurantCommentRepository , WriteRestaurantCommentRepository>();

        services.AddScoped<IReadOrderRatingRepository, ReadOrderRatingRepository>();
        services.AddScoped<IWriteOrderRatingRepository , WriteOrderRatingRepository>();

        services.AddScoped<IReadWorkerRepository, ReadWorkerRepository>();
        services.AddScoped<IWriteWorkerRepository, WriteWorkerRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}