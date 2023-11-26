using DataAccess.Repositories;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddScoped<IRepository, WebinarRepository>();
        return services;
    }
}