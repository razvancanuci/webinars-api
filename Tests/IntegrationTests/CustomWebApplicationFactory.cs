using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IntegrationTests;

public class CustomWebApplicationFactory: WebApplicationFactory<Program>
{
    private DbContext _context;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, builder) =>
        {
            builder.AddJsonFile("appsettings.json");
            builder.AddUserSecrets<Program>();
        }).ConfigureServices(services =>
        {
            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<WebinarContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<WebApplicationFactory<Program>>>();
        
                db.Database.EnsureCreated();
                _context = db;

                try
                {
                    DatabaseSeed.InitializeDbForTests(db);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                                        "database with test messages. Error: {Message}", ex.Message);
                }
            }
        });
    }

    public new async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}