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
            this.deviceClient = DeviceClient.CreateFromConnectionString(config.IotHubConnectionString);
        }

        public async Task SendMessageToCloudAsync<T>(IotMessage<T> messageToSend)
        {
            try
            {
                logger.LogInformation("received Message. Forwarding to IoTHub");
                await deviceClient.OpenAsync();
                logger.LogInformation("IoT Hub connection opened successfully");
                var json = JsonSerializer.Serialize(messageToSend);
                var message = new Message(Encoding.UTF8.GetBytes(json));
                await deviceClient.SendEventAsync(message);
                logger.LogInformation("Successfully forwarded to IoTHub");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send message to IoT Hub");
                throw;
            }
        }
    }

    public record ConnectorConfig
    {
        public required string IotHubConnectionString { get; init; }
    }
}
