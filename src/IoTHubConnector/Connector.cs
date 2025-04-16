using System.Text;
using System.Text.Json;
using Interfaces;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;

namespace IoTHubConnector
{
    public class Connector
    {
        private readonly DeviceClient? _deviceClient;
        private readonly ILogger<Connector> _logger;

        public Connector(ILogger<Connector> logger, ConnectorConfig config)
        {
            _logger = logger;
            _deviceClient = !string.IsNullOrWhiteSpace(config.IotHubConnectionString)
                ? DeviceClient.CreateFromConnectionString(config.IotHubConnectionString)
                : null;

            if (_deviceClient is null)
            {
                _logger.LogWarning(
                    "No IoTHub connection string provided. No messages will be sent to IoTHub."
                );
            }
        }

        public async Task SendMessageToCloudAsync<T>(IotMessage<T> messageToSend)
        {
            if (_deviceClient is null)
            {
                return;
            }

            _logger.LogInformation("received Message. Forwarding to IoTHub");
            await _deviceClient.SendEventAsync(ConvertIotMessageToHubMessage(messageToSend));
            _logger.LogInformation("Succesfully forwarded to IoTHub");
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
