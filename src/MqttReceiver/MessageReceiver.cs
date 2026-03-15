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

            // Abonniere Nachrichten auf dem Topic "temperature/living_room"
            // Wenn eine Nachricht empfangen wird, deserialisiere sie und gib sie aus

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            // Trenne die Verbindung zum MQTT Broker
        }
    }
}
