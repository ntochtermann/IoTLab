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

        public event EventHandler<ControlMessageReceivedEventArgs>? ControlMessageReceived;

        public Connector(ILogger<Connector> logger, ConnectorConfig config)
        {
            this.logger = logger;
            this.deviceClient = DeviceClient.CreateFromConnectionString(
                config.IotHubConnectionString
            );
            this.deviceClient.SetReceiveMessageHandlerAsync(MessageHandler, null);
        }

        public async Task SendMessageToCloudAsync<T>(IotMessage<T> messageToSend)
        {
            logger.LogInformation("received Message. Forwarding to IoTHub");
            await deviceClient.SendEventAsync(ConvertIotMessageToHubMessage(messageToSend));
            logger.LogInformation("Succesfully forwarded to IoTHub");
        }

        private async Task<MessageResponse> MessageHandler(Message message, object userContext)
        {
            var controlMessage = ConvertHubMessageToControlMessage(message);
            if (controlMessage != null && ControlMessageReceived != null)
            {
                ControlMessageReceived(this, new ControlMessageReceivedEventArgs(controlMessage));
            }
            await deviceClient.CompleteAsync(message);

            return MessageResponse.Completed;
        }

        private static Message ConvertIotMessageToHubMessage<T>(IotMessage<T> messageToSend)
        {
            var jsonMessage = JsonSerializer.Serialize(messageToSend);
            return new Message(Encoding.UTF8.GetBytes(jsonMessage));
        }

        private static ControlMessage? ConvertHubMessageToControlMessage(Message message)
        {
            string messageContent = Encoding.UTF8.GetString(message.GetBytes());
            try
            {
                var controlMessage = JsonSerializer.Deserialize<ControlMessage>(messageContent);
                return controlMessage;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public record ConnectorConfig
    {
        public required string IotHubConnectionString { get; init; }
    }

    public class ControlMessageReceivedEventArgs : EventArgs
    {
        public ControlMessageReceivedEventArgs(ControlMessage controlMessage)
        {
            Message = controlMessage;
        }

        public ControlMessage Message { get; init; }
    }
}
