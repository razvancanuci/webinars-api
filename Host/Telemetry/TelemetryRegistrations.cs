﻿using WebinarAPI.Telemetry.Filters;

namespace WebinarAPI.Telemetry;

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
}