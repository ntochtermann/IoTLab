using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StreamProcessing;

namespace StreamProcessing
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStreamProcessing(this IServiceCollection services)
        {
            services.AddSingleton<Filter>();
            services.AddSingleton<AggregatorAverage>();
            services.AddSingleton<StreamProcessor>();
            return services;
        }
    }
}