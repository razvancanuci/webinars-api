using System.Reflection;
using Microsoft.Extensions.Configuration;
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
    .Build();

host.Run();