using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(config =>
    {
        var assembly = Assembly.GetExecutingAssembly();
        var executableLocation = Path.GetDirectoryName(assembly.Location);
        var configPath = Path.Combine(executableLocation, "..");
        
        config.AddJsonFile("local.settings.json", true, reloadOnChange: true)
            .AddJsonFile(Path.Combine(configPath, "appsettings.json"), true)
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .AddUserSecrets(assembly, true);
    })
    .ConfigureServices(services =>
    {
        services.AddHealthChecks()
            .AddAzureServiceBusSubscription(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration["ServiceBus:ConnectionString"]!;

                return connectionString;
            },
            provider => "send-emails-topic",
                provider => "send-email-cancellation-subscription")
            .AddAzureServiceBusSubscription(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration["ServiceBus:ConnectionString"]!;

                return connectionString;
            },
            provider => "send-emails-topic",
            provider => "send-email-registration-subscription");
    })
    .Build();
host.Run();