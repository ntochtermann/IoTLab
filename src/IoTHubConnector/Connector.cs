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
            this.deviceClient = DeviceClient.CreateFromConnectionString(
                config.IotHubConnectionString
            );
        }

        public async Task SendMessageToCloudAsync<T>(IotMessage<T> messageToSend)
        {
            logger.LogInformation("received Message. Forwarding to IoTHub");
            await deviceClient.SendEventAsync(ConvertIotMessageToHubMessage(messageToSend));
            logger.LogInformation("Succesfully forwarded to IoTHub");
        }
        
        private static Message ConvertIotMessageToHubMessage<T>(IotMessage<T> messageToSend)
        {
            var jsonMessage = JsonSerializer.Serialize(messageToSend);
            return new Message(Encoding.UTF8.GetBytes(jsonMessage));
        }
    }

    public record ConnectorConfig
    {
        public required string IotHubConnectionString { get; init; }
    }
}
