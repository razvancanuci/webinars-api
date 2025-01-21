using DataAccess;
using Domain.Interfaces;
using IntegrationTests.Shims;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests;

public class CustomWebApplicationFactory: WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, builder) =>
        {
            builder.AddJsonFile("appsettings.integrationTests.json");
        })
        .ConfigureServices((context, services) =>
        {
            services.AddDbContext<WebinarContext>(options =>
            {
                options.UseInMemoryDatabase("AppDb");
            });
            services.ReplaceScoped<IContentModerationService, ContentModerationServiceShim>();
            services.AddMassTransitTestHarness();
        });
    }
}