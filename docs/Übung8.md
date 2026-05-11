# Übung 8: Datenvisualisierung mit Grafana und InfluxDB

## Ziel der Übung

- Verstehen einer lokalen IoT-Datenvisualisierungs-Pipeline.
- Einsatz von Telegraf als MQTT-Subscriber und Datenbankschreiber.
- Speicherung von Zeitreihendaten in InfluxDB.
- Erstellung von Dashboards in Grafana zur Visualisierung der Sensordaten.

**Ausgangslage:**

Das System aus den vorherigen Übungen sendet Temperaturnachrichten über den MQTT-Broker.

**Zielbild:**

```text
MqttSender --> Mosquitto (MQTT) --> Telegraf --> InfluxDB --> Grafana
```

Alle Komponenten werden über Docker Compose gestartet. Die Temperaturdaten sind in Echtzeit im Grafana-Dashboard sichtbar.

---

## Schritt 1: Infrastruktur starten

Alle benötigten Services (Mosquitto, Telegraf, InfluxDB, Grafana) sind im **Dev Container** vorkonfiguriert.
In GitHub Codespaces sollten Sie die Oberflächen über die weitergeleiteten Ports im **Ports**-Panel öffnen, nicht über `localhost` innerhalb des Containers.

Folgende Services stehen bereit:

| Service   | Beschreibung                | URL / Port                     |
| :-------- | :-------------------------- | :----------------------------- |
| Mosquitto | MQTT-Broker                 | `localhost:1883`               |
| InfluxDB  | Zeitreihendatenbank         | weitergeleiteter Port `8086`   |
| Telegraf  | Verbindet MQTT mit InfluxDB | –                              |
| Grafana   | Visualisierungs-Dashboards  | weitergeleiteter Port `3000`   |

Login-Daten für InfluxDB und Grafana:  
**Benutzername:** `admin`  
**Passwort:** `MyPass@word`

Starten Sie nun den **MqttSender** lokal (F5 oder `dotnet run` im Projekt `src/MqttSenders`), um simulierte Temperaturdaten zu erzeugen.

---

## Schritt 2: InfluxDB erkunden

1. Öffnen Sie InfluxDB über den weitergeleiteten Port `8086` im **Ports**-Panel und melden Sie sich an.
2. Navigieren Sie zu **Data Explorer**.
3. Suchen Sie nach dem Bucket `CurrentData` und dem topic `temperature/living_room`.
4. Vergewissern Sie sich, dass Daten mit dem Feld `Message` (Temperaturwert) ankommen.

---

## Schritt 3: Grafana-Dashboard erkunden

1. Öffnen Sie Grafana über den weitergeleiteten Port `3000` im **Ports**-Panel und melden Sie sich an.
2. Navigieren Sie zu **Dashboards → IoT Temperature Dashboard**.
3. Das Panel zeigt die Durchschnittstemperatur der letzten 15 Minuten in 10-Sekunden-Intervallen.
4. Beobachten Sie, wie sich die simulierte Temperatur im Laufe der Zeit verändert (das Gerät wechselt zwischen Heiz- und Kühlmodus).

---

## Schritt 4: Eigenes Panel auf eigenem Dashboard erstellen

Erstellen Sie in Grafana ein zweites Dashboard (das erste kann nicht editiert werden, da es automatisch erzeugt wird), mit einem Panel, das den **aktuellen (letzten) Temperaturwert** als Gauge (Tachometer) anzeigt.

Hinweise:

- Fügen Sie ein neues Panel vom Typ **Gauge** hinzu.
- Verwenden Sie folgendes Flux-Query als Ausgangspunkt:

```flux
from(bucket: "CurrentData")
  |> range(start: -1m)
  |> filter(fn: (r) => r["topic"] == "temperature/living_room")
  |> filter(fn: (r) => r["_field"] == "Message")
  |> last()
```

- Stellen Sie die Einheit auf **Celsius (°C)** ein.
- Definieren Sie sinnvolle Schwellenwerte (z. B. grün bis 22 °C, gelb bis 28 °C, rot ab 28 °C).

---

## Schritt 5: Temperaturbereiche als Pie Chart auswerten

Erstellen Sie ein Panel, das zeigt, **wie viele Messwerte** in den letzten 15 Minuten in die Bereiche **kuehl**, **komfortabel** und **heiss** fallen.

Hinweise:

- Fügen Sie ein neues Panel vom Typ **Pie chart** hinzu.
- Verwenden Sie folgendes Flux-Query als Ausgangspunkt:

```flux
from(bucket: "CurrentData")
  |> range(start: -15m)
  |> filter(fn: (r) => r["topic"] == "temperature/living_room")
  |> filter(fn: (r) => r["_field"] == "Message")
```

- Legen Sie aussagekraeftige Farben fest, z. B. blau für `kuehl`, grün für `komfortabel`, rot für `heiss`.
- Aktivieren Sie die Anzeige von **Wert und Prozentanteil**.

---

## Schritt 6: Temperaturverteilung als Heatmap untersuchen

Erstellen Sie ein Panel, das die **Verteilung der Temperaturwerte über die Zeit** als **Heatmap** zeigt.

Hinweise:

- Fügen Sie ein neues Panel vom Typ **Heatmap** hinzu.
- Verwenden Sie zuerst folgende Abfrage, um die Rohwerte der letzten 30 Minuten zu laden:

```flux
from(bucket: "CurrentData")
  |> range(start: -30m)
  |> filter(fn: (r) => r["topic"] == "temperature/living_room")
  |> filter(fn: (r) => r["_field"] == "Message")
```

- Stellen Sie die Y-Achse auf **Celsius (°C)** ein.
- Waehlen Sie eine sinnvolle Bucket-Groesse, damit Verdichtungen im Bereich der typischen Temperaturen sichtbar werden.
- Vergleichen Sie die Heatmap mit dem Time-Series-Panel aus Schritt 3.

---

## Bonus: Alert in Grafana einrichten

Konfigurieren Sie in Grafana einen **Alert**, der ausgelöst wird, wenn die Temperatur über **27 °C** steigt.

- Navigieren Sie im Panel zu **Alert → New alert rule**.
- Definieren Sie die Bedingung basierend auf dem Flux-Query aus Schritt 4.
- Wählen Sie als Notification-Channel die Option **Grafana managed alert**.
