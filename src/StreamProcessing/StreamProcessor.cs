using Interfaces;
using IoTHubConnector;
using Microsoft.Extensions.Logging;

namespace StreamProcessing
{
    public class StreamProcessor
    {
        private readonly Filter filter;
        private readonly AggregatorAverage aggregator;
        private readonly Connector ioTHubConnector;
        private readonly ILogger<StreamProcessor> logger;

        public StreamProcessor(
            Filter filter,
            AggregatorAverage aggregator,
            Connector ioTHubConnector,
            ILogger<StreamProcessor> logger
        )
        {
            this.filter = filter;
            this.aggregator = aggregator;
            this.ioTHubConnector = ioTHubConnector;
            this.logger = logger;
        }

        public async Task HandleMessage(IotMessage<double> message)
        {
            bool filterResult = filter.HandleMessage(message);

            if (filterResult)
            {
                IotMessage<double>? aggregatorResult = aggregator.HandleMessage(message);

                if (aggregatorResult != null)
                {
                    // Process the aggregated result
                    logger.LogInformation("Aggregated value: {Message}", aggregatorResult.Message);
                    logger.LogInformation("Sending message to IoT Hub");
                    await ioTHubConnector.SendMessageToCloudAsync(aggregatorResult);
                }
            }
        }
    }
}
