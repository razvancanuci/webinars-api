using DataAccess.Repositories;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DataAccess;

public static class ServicesExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Webinar>, WebinarRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<WebinarContext>((provider, options) =>
        {
            var dbSettings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            options.UseCosmos(
                dbSettings.ConnectionString,
                dbSettings.DbName
            );
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