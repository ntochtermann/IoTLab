using System.Text;
using System.Text.Json;
using Interfaces;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;

namespace IoTHubConnector
{
    public class Connector
    {
        private readonly DeviceClient deviceClient;
        private readonly ILogger<Connector> logger;

        public Connector(ILogger<Connector> logger, ConnectorConfig config)
        {
            this.logger = logger;
            // TODO: Initialize the deviceClient using the IoT Hub connection string from the config.
        }

        public async Task SendMessageToCloudAsync<T>(IotMessage<T> messageToSend)
        {
            // TODO: Convert the message to a IoTHub message
            logger.LogInformation("received Message. Forwarding to IoTHub");
            // TODO: Implement the logic to send the message to IoT Hub using the deviceClient.
            logger.LogInformation("Succesfully forwarded to IoTHub");
        }
    }

    public record ConnectorConfig
    {
        public required string IotHubConnectionString { get; init; }
    }
}
