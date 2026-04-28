using System.Text.Json;
using Interfaces;
using MQTTnet;
using StreamProcessing;

namespace MqttReceiver
{
    public class MessageReceiver(ILogger<MessageReceiver> logger, StreamProcessor processor) : BackgroundService
    {
        private readonly ILogger<MessageReceiver> _logger = logger;
        private readonly StreamProcessor processor = processor;
        private readonly MqttClientFactory _mqttFactory = new();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var mqttClient = _mqttFactory.CreateMqttClient();
            var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("localhost", 1883).Build();
            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            mqttClient.ApplicationMessageReceivedAsync += async (e) =>
            {
                _logger.LogInformation("Received message from topic: {topic}", e.ApplicationMessage.Topic);
                var message = JsonSerializer.Deserialize<IotMessage<double>>(e.ApplicationMessage.ConvertPayloadToString());
                if (message is null)
                {
                    _logger.LogError("Received null message");
                    return;
                }

                _logger.LogInformation("Received Message: {message}, Timestamp: {Timestamp}, Type: {Type}", message.Message, message.Timestamp, message.Type);
                try
                {
                    await processor.HandleMessage(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                }
                return;
            };

            await mqttClient.SubscribeAsync(
                new MqttTopicFilterBuilder()
                    .WithTopic("temperature/living_room")
                    .Build(),
                stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            await mqttClient.DisconnectAsync(
                new MqttClientDisconnectOptionsBuilder()
                    .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
                    .Build(),
                stoppingToken);
        }
    }
}
