using Application.Repositories;
using Application.Repositories.CourierRepository;
using Application.Services;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repositories;
using Persistence.Repositories.CourierRepository;
using Persistence.Services;

public static class Extension
{

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICourierService, CourierService>();
        services.AddScoped<IWorkerService, WorkerService>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IReadCourierRepository, ReadCourierRepository>();
        services.AddScoped<IWriteCourierRepository, WriteCourierRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}