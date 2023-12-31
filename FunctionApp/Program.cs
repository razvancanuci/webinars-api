using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("local.settings.json", true)
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables();
    })
    .Build();

host.Run();