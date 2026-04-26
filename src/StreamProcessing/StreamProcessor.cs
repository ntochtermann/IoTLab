using Interfaces;
using Microsoft.Extensions.Logging;

namespace StreamProcessing
{
    public class StreamProcessor
    {
        private readonly Filter filter;
        private readonly AggregatorAverage aggregator;
        private readonly ILogger<StreamProcessor> logger;

        public StreamProcessor(Filter filter, AggregatorAverage aggregator, ILogger<StreamProcessor> logger)
        {
            this.filter = filter;
            this.aggregator = aggregator;
            this.logger = logger;
        }

        public Task HandleMessage(IotMessage<double> message)
        {
            bool filterResult = filter.HandleMessage(message);

            if (filterResult)
            {
                IotMessage<double>? aggregatorResult = aggregator.HandleMessage(message);

                if (aggregatorResult != null)
                {
                    // Process the aggregated result
                    logger.LogInformation($"Aggregated value: {aggregatorResult.Message}");
                    logger.LogInformation($"Sending message to IoT Hub");
                }
            }
            return Task.CompletedTask;
        }
    }
}
