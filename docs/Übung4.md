# Integration eines OPC UA Servers

## Ziel der Übung

In dieser Übung erweitern Sie das bestehende System aus der "MQTTSenders"- und der "MQTTReceiver"-Console-Applikation. Sie werden einen OPC UA Server Simulator mittels Docker aufsetzen und den "MQTTSenders" so anpassen, dass er sich mit diesem Server verbindet, auf einen spezifizierten Node subscribed und seine Daten über MQTT weiterleitet.

### Schritt 1: Aufsetzen des OPC UA Server Simulator

Docker Compose starten

```bash
docker compose up
```

Überprüfung der Funktionsfähigkeit: Stellen Sie sicher, dass der OPC UA Server korrekt läuft und erreichbar ist. z.B. mithilfe von [Free OPC UA](https://github.com/FreeOpcUa/opcua-client-gui).

### Schritt 2: Anpassen des "MQTTSenders"

- Integration des OPC UA-Clients: Erweitern Sie Ihre "MQTTSenders"-Applikation, sodass sie auch als OPC UA-Client agieren kann. Nutzen Sie dazu die vorgegebene Klasse *OpaUASessionFactory.cs* um eine Session mit dem OPC Server aufzusetzen
- Node-Subscription einrichten: Abonnieren Sie den vorgegebenen Node (NodeID = **ns=3;s=SpikeData**) des OPC UA Servers, um dessen Daten zu empfangen.

### Schritt 3: Weiterleitung der Daten an den MQTT-Broker

- Datenempfang implementieren: Empfangen Sie die Daten vom abonnierten Node des OPC UA Servers mittels einer Callback-Funktion oder einer regelmäßigen Abfrage.
- MQTT-Publishing realisieren: Leiten Sie die empfangenen Daten an den Mosquitto MQTT Broker unter Verwendung eines bestimmten Topics (frei wählbar) weiter.

### Schritt 4: Modifikation des "MQTTReceiver"

- Anpassung des Datenempfangs: Stellen Sie sicher, dass die "MQTTReceiver"-Applikation auf das Topic hört, unter dem der "MQTTSenders" die OPC-Daten veröffentlicht.
- Verarbeitung der Streams: Führen Sie die erforderlichen Datenverarbeitungen durch, wie zum Beispiel Transformation, Aggregation oder Analyse der Daten.

## Schritt 5: Testen der Gesamtapplikation

- Ausführung des Testlaufs: Starten Sie sowohl den "MQTTSenders" als auch den "MQTTReceiver" und stellen Sie sicher, dass der gesamte Datenfluss funktioniert - von der Datenerfassung am OPC UA Server bis hin zur Verarbeitung im "MQTTReceiver".
- Debugging: Lokalisieren und beheben Sie Fehler, um sicherzustellen, dass die Datenkette wie gewünscht arbeitet.
