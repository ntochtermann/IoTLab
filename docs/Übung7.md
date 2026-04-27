# Cloud to Device Communication

## IoTHub Setup

Wir verwenden in dieser Übung den Built in Endpoint des IoTHubs.
Damit die Nachrichten dorthin geroutet werden, müssen alle anderen Routen gelöscht werden.

`Azure Portal --> IoTHub --> Message Routing --> Alle Routen löschen`

## App Konfiguration

Damit die Applikation die korrekte Verbindung aufbauen kann, müssen ein paar Sachen konfiguriert werden.
In die `appsettings.json` des MqttReceivers muss der IoTHubConnectionString eingefügt werden.
Damit ist der Device Connection String gemeint.

`Azure Portal --> IoTHub --> Devices --> Euer Device --> Primary Connection String`

Außerdem müssen in der CommandCreatorFunction local.settings.json folgende Werte gesetzt werden

```json
    "IotHubEndpoint": "<your Event Hub-compatible endpoint>",
    "IotHubEndpointName": "<your Event Hub-compatible name>",
    "IoTHubDeviceId": "<your IoT Hub Device ID here>",
    "IoTHubServiceConnectionString": "<your IoT Hub Service Connection String here>"
```

1. IoTHubDeviceId:
    1. `Azure Portal --> IoTHub --> Devices --> Euer Device --> Device ID`
1. IotHubEndpointName:
    1. `Azure Portal --> IoTHub --> Built-in endpoints --> Event Hub-compatible name`
1. IotHubEndpoint:
    1. `Azure Portal --> IoTHub --> Built-in endpoints --> Event Hub-compatible endpoint`
1. IoTHubServiceConnectionString:
    1. `Azure Portal --> IoTHub --> Shared access policies --> service --> primary connection string`

## Methoden

Folgende Methoden müssen implementiert werden:

1. `HandleControlMessage` in `MqttDeviceSimulator`
1. `SendColderMessage/SendWarmerMessage` in `CommandCreatorFunction.MessageHandler`
1. ``HandleControlMessageReceivedEvent`` in ``MessageReceiver``

## Start Projekte

Zum vollständigen Testen müssen 3 Projekte gestartet werden.

1. MqttSenders
1. MqttReceiver
1. CommandCreatorFunction

Außerdem muss der MQTT Broker in Docker gestartet werden.

## Bonus: Azure Function Deployen

Wenn die Azure Function funktional fertig ist, kann sie in Azure deployt werden.
Dazu muss eine Azure Function Resource angelegt werden.
In Visual Studio habt ihr die Option die Function direkt zu deployen.
Dazu auf das Projekt rechtsklicken und Publish auswählen und dem Dialog folgen.
Wenn die Azure Function erfolgreich deployt wurde, muss das Projekt nicht mehr lokal gestartet werden, weil es in der Cloud läuft.
