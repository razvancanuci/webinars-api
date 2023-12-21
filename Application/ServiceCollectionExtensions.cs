
using Application.Handlers.Queries;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAvailableWebinarsQueryHandler).Assembly));
        services.RegisterMapsterConfiguration();
        services.AddScoped<ICacheService, CacheService>();
        return services;
    }
    
    private static IServiceCollection RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<Webinar, WebinarShortInfoDto>.NewConfig();
        TypeAdapterConfig<Webinar, WebinarInfoDto>.NewConfig();
        return services;
    }
}