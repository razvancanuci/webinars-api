using System.Diagnostics.CodeAnalysis;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AzureStorage;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureBlobStorage(this IServiceCollection services)
    {
        services.AddScoped<IFileStorage, AzureBlobStorage>();
        return services;
    }
}