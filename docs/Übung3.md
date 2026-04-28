# Übung 3: IoT Hub Connector

In dieser Übung implementieren Sie die Klasse `Connector`, die Nachrichten an Azure IoT Hub sendet.

## Aufgabe 1: Erstellen eines IoT Hubs in Azure

1. Melden Sie sich im [Azure-Portal](https://portal.azure.com) an.
2. Navigieren Sie zu **"Ressource erstellen"** und suchen Sie nach **"IoT Hub"**.
3. Wählen Sie **"Erstellen"** und geben Sie die erforderlichen Informationen ein
4. Klicken Sie auf **"Überprüfen + Erstellen"** und dann auf **"Erstellen"**.

## Aufgabe 2: Abrufen des ConnectionStrings

1. Öffnen Sie Ihr IoT Hub im Azure-Portal.
2. Navigieren Sie zu **"Geräteverwaltung" > "Geräte"**.
3. Klicken Sie auf **"Neues Gerät hinzufügen"** und geben Sie einen Gerätenamen ein.
4. Nach der Erstellung des Geräts klicken Sie auf den Gerätenamen.
5. Kopieren Sie die **Primäre Verbindungszeichenfolge**.
6. Fügen Sie den ConnectionString in die Datei `appsettings.json` unter `"IoTHubConnectionString"` ein.

## Aufgabe 3: Implementierung der Klasse `Connector`

Die Klasse `Connector` ist für die Kommunikation mit Azure IoT Hub verantwortlich. Implementieren Sie die Klasse.
