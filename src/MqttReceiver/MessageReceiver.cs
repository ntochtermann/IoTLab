using System.Text.Json;
using Interfaces;
using MQTTnet;

namespace MqttReceiver
{
    public class MessageReceiver(ILogger<MessageReceiver> logger) : BackgroundService
    {
        private readonly ILogger<MessageReceiver> _logger = logger;
        private readonly MqttClientFactory _mqttFactory = new();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Verbinde dich mit dem MQTT Broker über localhost und Port 1883
            var mqttClient = _mqttFactory.CreateMqttClient();
            var options = _mqttFactory.CreateClientOptionsBuilder().WithTcpServer("localhost", 1883).Build();
            await mqttClient.ConnectAsync(options, CancellationToken.None);

            // Abonniere Nachrichten auf dem Topic "temperature/living_room"
            var subscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder().WithTopicFilter("temperature/living_room").Build();
            await mqttClient.SubscribeAsync(subscribeOptions, CancellationToken.None);

            // Wenn eine Nachricht empfangen wird, deserialisiere sie und gib sie aus
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
                return;
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            // Trenne die Verbindung zum MQTT Broker
            await mqttClient.DisconnectAsync(new MqttClientDisconnectOptionsBuilder().WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection).Build(), CancellationToken.None);
        }
    }
}
