using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging.ApplicationInsights;
using WebinarAPI.Telemetry.Filters;

namespace WebinarAPI.Telemetry;

[ExcludeFromCodeCoverage]
public static class TelemetryRegistrations
{
    public static IServiceCollection AddAppInsights(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationInsightsTelemetry(options =>
        {
            options.ConnectionString = configuration["AppInsights:ConnectionString"]!;
        });
        
        services.AddApplicationInsightsTelemetryProcessor<DependencyTelemetryFilter>();
        
        return services;
    }

    public static ILoggingBuilder AddAppInsightsLogs(this ILoggingBuilder builder, IConfiguration configuration)
    {
        builder.AddConsole();
        builder.SetMinimumLevel(LogLevel.Information);
        
        builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
        
        builder.AddApplicationInsights(
            configureTelemetryConfiguration : config=> config.ConnectionString = configuration["AppInsights:ConnectionString"],
            configureApplicationInsightsLoggerOptions: options => { });
        
        return builder;
    }
}