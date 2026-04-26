using System.Text.Json;
using Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;

namespace MqttSenders
{
    public class MqttDeviceSimulator(ILogger<MqttDeviceSimulator> logger) : BackgroundService
    {
        private readonly ILogger<MqttDeviceSimulator> _logger = logger;
        private readonly Random _random = new();
        private readonly MqttClientFactory _mqttFactory = new();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Verbinde dich mit dem MQTT Broker über localhost und Port 1883
            var mqttClient = _mqttFactory.CreateMqttClient();
            var options = _mqttFactory.CreateClientOptionsBuilder().WithTcpServer("localhost", 1883).Build();
            await mqttClient.ConnectAsync(options, CancellationToken.None);

            while (!stoppingToken.IsCancellationRequested)
            {
                var temperature = _random.NextInt64(-10, 40);
                var iotMessage = new IotMessage<long>(temperature, DateTimeOffset.Now, "telemetry");
                var payload = JsonSerializer.Serialize(iotMessage);

                // Erstelle eine MQTT Nachricht mit dem Topic "temperature/living_room" und dem Payload der Temperatur
                var message = new MqttApplicationMessageBuilder().WithTopic("temperature/living_room").WithPayload(payload).Build();

                _logger.LogInformation(
                    "Publishing Message at {Timestamp} with Temperature: {Temperature}",
                    DateTimeOffset.Now,
                    temperature
                );

                // Sende die MQTT Nachricht an den Broker
                await mqttClient.PublishAsync(message, CancellationToken.None);

                await Task.Delay(1000, stoppingToken);
            }

            // Trenne die Verbindung zum MQTT Broker
            await mqttClient.DisconnectAsync(new MqttClientDisconnectOptionsBuilder().WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection).Build(), CancellationToken.None);
        }
    }
}
