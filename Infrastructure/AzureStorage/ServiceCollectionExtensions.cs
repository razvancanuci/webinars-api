using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AzureStorage;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureBlobStorage(this IServiceCollection services)
    {
        services.AddScoped<IFileStorage, AzureBlobStorage>();
        return services;
    }
}