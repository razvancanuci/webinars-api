
using System.Diagnostics.CodeAnalysis;
using Application.Handlers.Queries;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Application.Validators;
using Domain.Dtos;
using Domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAvailableWebinarsQueryHandler).Assembly));
        services.RegisterMapsterConfiguration();
        services.AddFluentValidation();
        services.AddScoped<ICacheService, CacheService>();
        
        return services;
    }
    
    private static IServiceCollection RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<Webinar, WebinarShortInfoDto>.NewConfig();
        TypeAdapterConfig<Webinar, WebinarInfoDto>
            .NewConfig()
            .MapWith(src => new WebinarInfoDto{
                Title = src.Title,
                Host = src.Host,
                Description = src.Description,
                ScheduledAt = src.ScheduledAt
            });
        return services;
    }

    private static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<NewWebinarRequestValidator>();
        services.AddFluentValidationAutoValidation();

        return services;
    }
}