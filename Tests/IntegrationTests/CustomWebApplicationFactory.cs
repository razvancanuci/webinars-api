using Domain.Interfaces;
using IntegrationTests.Shims;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

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
            services.ReplaceScoped<IContentModerationService, ContentModerationServiceShim>();
            services.AddMassTransitTestHarness();
        });
    }
}