using Domain.Interfaces;
using Domain.Settings;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AIServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAiServices(this IServiceCollection services)
    {
        services.AddScoped<IContentModerationService>(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<ContentModeratorSettings>>().Value;
            var client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(settings.Key));
            client.Endpoint = settings.Endpoint;

            return new ContentModerationService(client);
        });
        
        return services;
    }
}