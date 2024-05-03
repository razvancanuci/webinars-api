using System.Diagnostics.CodeAnalysis;
using Domain.Interfaces;
using Domain.Messages;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
    
            x.AddConfigureEndpointsCallback((name, cfg) =>
            {
                cfg.UseMessageRetry(r => r.Immediate(2));
            });
    
            x.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(configuration["ServiceBus:ConnectionString"]);
        
                cfg.Message<EmailRegistrationMessage>(
                    x =>
                    {
                        x.SetEntityName(configuration["ServiceBus:SendEmailTopicName"]!);
                    });
        
                cfg.Message<EmailCancellationMessage>(
                    x =>
                    {
                        x.SetEntityName(configuration["ServiceBus:SendEmailTopicName"]!);
                    });
                cfg.ConfigureEndpoints(context);
            });
        });
        
        services.AddScoped<IMessageService, MessageService>();
        return services;
    }
}