using FunctionApp;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(
        (hostContext, config) =>
        {
            if (hostContext.HostingEnvironment.IsDevelopment())
            {
                config.AddJsonFile("local.settings.json");
                config.AddUserSecrets<Program>();
            }
        }
    )
    .ConfigureServices(
        (hostContext, services) =>
        {
            services.AddApplicationInsightsTelemetryWorkerService();

            services.ConfigureFunctionsApplicationInsights();
        }
    )
    .Build();
host.Run();
