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
            using var mqttClient = _mqttFactory.CreateMqttClient();
            var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("localhost", 1883).Build();
            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            while (!stoppingToken.IsCancellationRequested)
            {
                var temperature = _random.NextInt64(-10, 40);
                var iotMessage = new IotMessage<long>(temperature, DateTimeOffset.Now, "telemetry");
                var payload = JsonSerializer.Serialize(iotMessage);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("temperature/living_room")
                    .WithPayload(payload)
                    .Build();

                _logger.LogInformation("Publishing Message at {Timestamp} with Temperature: {Temperature}", DateTimeOffset.Now, temperature);
                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
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
