using System.Text;
using System.Text.Json;
using Azure.Messaging.EventHubs;
using Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CommandCreatorFunction
{
    public class CommandCreatorFunction(ILogger<CommandCreatorFunction> logger, MessageHandler messageHandler)
    {
        [Function(nameof(CommandCreatorFunction))]
        public async Task Run([EventHubTrigger("%IotHubEndpointName%", Connection = "IotHubEndpoint")] EventData[] events)
        {
            foreach (EventData @event in events)
            {
                var jsonMessage = Encoding.UTF8.GetString(@event.Body.ToArray());
                logger.LogInformation("Event Body: {body}", jsonMessage);
                logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
                var iotMessage = ParseFromJson(jsonMessage);
                await messageHandler.HandleMessageAsync(iotMessage);
            }
        }

        private static IotMessage<double> ParseFromJson(string json)
        {
            return JsonSerializer.Deserialize<IotMessage<double>>(json) ?? throw new ArgumentException($"Could not parse message from json {json}");
        }
    }
}
