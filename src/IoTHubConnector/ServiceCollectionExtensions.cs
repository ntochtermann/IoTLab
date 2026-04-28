using Microsoft.Extensions.DependencyInjection;

namespace IoTHubConnector
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIotHubConnector(
            this IServiceCollection services,
            string iotHubConnectionString
        )
        {
            services.AddSingleton<Connector>();
            services.AddSingleton(_ => new ConnectorConfig
            {
                IotHubConnectionString = iotHubConnectionString,
            });

            return services;
        }
    }
}
