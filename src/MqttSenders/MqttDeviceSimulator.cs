using System.Text.Json;
using Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;

namespace MqttSenders
{
    public class MqttDeviceSimulator(ILogger<MqttDeviceSimulator> logger) : BackgroundService
    {
        private static readonly string[] _productCodes =
        {
            "Screws",
            "Nails",
            "Bolts",
            "Hammers",
            "Screwdrivers",
        };
        private readonly ILogger<MqttDeviceSimulator> _logger = logger;
        private readonly Random _random = new();
        private readonly MqttClientFactory _mqttFactory = new();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var mqttClient = _mqttFactory.CreateMqttClient();
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();
            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            while (!stoppingToken.IsCancellationRequested)
            {
                var quantity = _random.Next(1, 10);
                var productCode = _productCodes[_random.Next(_productCodes.Length)];
                var iotMessage = new ProducedMessage(DateTimeOffset.Now, productCode, quantity);
                var payload = JsonSerializer.Serialize(iotMessage);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("telemetry")
                    .WithPayload(payload)
                    .Build();

                _logger.LogInformation(
                    "Publishing Message at {Timestamp} with ProductCode: {ProductCode} and Quantity: {Quantity}",
                    DateTimeOffset.Now,
                    productCode,
                    quantity
                );

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                await Task.Delay(1000, stoppingToken);
            }

            await mqttClient.DisconnectAsync(
                new MqttClientDisconnectOptionsBuilder()
                    .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
                    .Build(),
                stoppingToken
            );
        }
    }
}
