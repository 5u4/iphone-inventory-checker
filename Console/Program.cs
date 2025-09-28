using Config.Net;
using IPhoneStockChecker.Console;
using IPhoneStockChecker.Console.ServiceConfigurations;
using IPhoneStockChecker.Console.Settings;
using Microsoft.Extensions.DependencyInjection;

var settings = new ConfigurationBuilder<IConsoleAppSettings>()
    .UseEnvironmentVariables()
    .UseDotEnvFile()
    .UseJsonFile("appsettings.json")
    .Build();

var serviceComponent = new ConsoleServiceComponent(settings);
serviceComponent.Verify();

var services = new ServiceCollection();
serviceComponent.Configure(services);

var serviceProvider = services.BuildServiceProvider();

var runner = serviceProvider.GetRequiredService<IConsoleRunner>();

await runner.Run();
