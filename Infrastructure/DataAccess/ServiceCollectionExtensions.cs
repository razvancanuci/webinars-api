using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        return services;
    }
}