using System.Text.Json;
using Config.Net;
using IPhoneStockChecker.Console.ServiceConfigurations;
using IPhoneStockChecker.Console.Settings;
using IPhoneStockChecker.Core.Workflows;
using Microsoft.Extensions.DependencyInjection;

var settings = new ConfigurationBuilder<IConsoleAppSettings>()
    .UseEnvironmentVariables()
    .UseDotEnvFile()
    .UseJsonFile("appsettings.json")
    .Build();

Console.WriteLine($"{JsonSerializer.Serialize(settings)}");

var serviceComponent = new ConsoleServiceComponent(settings);
serviceComponent.Verify();

var services = new ServiceCollection();
serviceComponent.Configure(services);

var serviceProvider = services.BuildServiceProvider();

var workflowFactory = serviceProvider.GetRequiredService<IWorkflowFactory>();

var workflow = workflowFactory.Create();

await workflow.Run();
