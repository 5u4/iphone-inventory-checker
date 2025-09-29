using DotNetEnv;
using IPhoneStockChecker.Console;
using IPhoneStockChecker.Console.ServiceConfigurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

Env.Load();

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, false)
    .AddEnvironmentVariables()
    .Build();

var serviceComponent = new ConsoleServiceComponent(config);
serviceComponent.Verify();

var services = new ServiceCollection();
serviceComponent.Configure(services);
services.AddLogging(builder => builder.AddConsole());

var serviceProvider = services.BuildServiceProvider();

var runner = serviceProvider.GetRequiredService<IConsoleRunner>();

await runner.Run();
