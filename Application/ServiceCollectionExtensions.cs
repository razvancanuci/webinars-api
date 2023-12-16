
using Application.Handlers.Queries;
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
        return services;
    }
    
    private static IServiceCollection RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<Webinar, WebinarShortInfoDto>
            .NewConfig();
        return services;
    }
}