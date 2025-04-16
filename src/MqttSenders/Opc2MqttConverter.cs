using System.Text.Json;
using Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using Opc.Ua;
using Opc.Ua.Client;

namespace MqttSenders
{
    public class Opc2MqttConverter : BackgroundService
    {
        private readonly ILogger<Opc2MqttConverter> _logger;
        private readonly MqttClientFactory _mqttFactory = new();
        private IMqttClient _mqttClient;

        public Opc2MqttConverter(ILogger<Opc2MqttConverter> logger)
        {
            _logger = logger;
            _mqttClient = _mqttFactory.CreateMqttClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .WithClientId("opc2mqtt")
                .Build();
            await _mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            // Erstellung einer OPC-UA-Sitzung und Verbindungsaufbau
            var sessionFactory = new OpcUASessionFactory("opc.tcp://opcplc:50000/");

            // Session erstellen

            // Erstellung einer Subscription mit einem Veröffentlichungsintervall von 5 Sekunden
            // var subscription = new Subscription(...);

            // Hinzufügen des zu überwachenden Nodes zur Subscription
            // var monitoredItem = new MonitoredItem(...);

            // Ereignisbehandlung für neue Benachrichtigungen (OnNotificationAsync)
            // .Notification += OnNotificationAsync;

            // Monitored Item der Subscription hinzufügen
            // .AddItem

            // Subscription zur Session hinzufügen
            // .AddSubscription

            // Subscription erstellen
            // .CreateAsync

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            await _mqttClient.DisconnectAsync(
                new MqttClientDisconnectOptionsBuilder()
                    .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
                    .Build(),
                stoppingToken
            );

            _mqttClient.Dispose();
        }

        // Ereignisbehandlung für neue Benachrichtigungen von überwachten OPC-UA-Nodes
        private void OnNotificationAsync(
            MonitoredItem monitoredItem,
            MonitoredItemNotificationEventArgs e
        )
        {
            _logger.LogInformation(
                "Received message for MonitoredItem {DisplayName}",
                monitoredItem.DisplayName
            );

            // Überprüfen, ob die Benachrichtigung vom unterstützten Typ ist (hier: Double)
            var notification = e.NotificationValue as MonitoredItemNotification;
            if (notification == null)
            {
                _logger.LogWarning(
                    "Received message for MonitoredItem {DisplayName} could not be casted correctly",
                    monitoredItem.DisplayName
                );
                return;
            }

            var supportedType =
                notification.Value.WrappedValue.TypeInfo.BuiltInType == BuiltInType.Double;

            // Wenn der Typ nicht unterstützt wird, eine entsprechende Meldung protokollieren
            if (!supportedType)
            {
                _logger.LogInformation(
                    "Received message for MonitoredItem {DisplayName} is not sending supported type",
                    monitoredItem.DisplayName
                );
                return;
            }

            // Den Double-Wert aus der Benachrichtigung abrufen und in einen IoT-Nachricht umwandeln
            var doubleValue = Convert.ToDouble(notification.Value.ToString());
            var iotMessage = new IotMessage<double>(
                doubleValue,
                DateTimeOffset.Now,
                monitoredItem.DisplayName
            );
            _logger.LogInformation(
                "Publishing OPC UA Message at {Timestamp} with Value: {Value}",
                DateTimeOffset.Now,
                doubleValue
            );

            var payload = JsonSerializer.Serialize(iotMessage);

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic("opc/nodeValues")
                .WithPayload(payload)
                .Build();

            _mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
        }
    }
}
