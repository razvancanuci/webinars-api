using DataAccess.Repositories;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRepository<Webinar>, WebinarRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<WebinarContext>(options =>
            options.UseCosmos(
                configuration["Database:ConnectionString"],
                configuration["Database:DbName"]
            ));
        return services;
    }
}