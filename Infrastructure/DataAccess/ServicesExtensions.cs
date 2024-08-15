using System.Diagnostics.CodeAnalysis;
using DataAccess.Interceptors;
using DataAccess.Repositories;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DataAccess;

[ExcludeFromCodeCoverage]
public static class ServicesExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddSingleton<WebinarContextInterceptor>();
        services.AddScoped<IWebinarRepository, WebinarRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<WebinarContext>((provider, options) =>
        {
            var dbSettings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            options.UseCosmos(
                dbSettings.ConnectionString,
                dbSettings.DbName
            )
            .AddInterceptors(provider.GetRequiredService<WebinarContextInterceptor>());
        });
           
        return services;
    }

    public static IServiceProvider AutoCreateDb(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WebinarContext>();
        context.Database.EnsureCreated();

        return serviceProvider;
    }
}