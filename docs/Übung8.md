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

| Service   | Beschreibung                | URL / Port                        |
|-----------|-----------------------------|-----------------------------------|
| Mosquitto | MQTT-Broker                 | `localhost:1883`                  |
| InfluxDB  | Zeitreihendatenbank         | weitergeleiteter Port `38086`     |
| Telegraf  | Verbindet MQTT mit InfluxDB | –                                 |
| Grafana   | Visualisierungs-Dashboards  | weitergeleiteter Port `3000`      |

Login-Daten für InfluxDB und Grafana:  
**Benutzername:** `admin`  
**Passwort:** `MyPass@word`

Starten Sie nun den **MqttSender** lokal (F5 oder `dotnet run` im Projekt `src/MqttSenders`), um simulierte Temperaturdaten zu erzeugen.

---

## Schritt 2: Datenpipeline verstehen

Machen Sie sich mit den Konfigurationsdateien im Ordner `.devcontainer/` vertraut:

- **`mosquitto.conf`** – Konfiguration des MQTT-Brokers (Port, anonymer Zugriff).
- **`telegraf.conf`** – Telegraf abonniert alle MQTT-Topics (`#`) und schreibt die Daten in InfluxDB.
- **`grafana-datasources.yaml`** – Verbindet Grafana automatisch mit InfluxDB.
- **`grafana-dashboard.json`** – Vorkonfiguriertes Dashboard mit einem Temperatur-Graphen.

Beantworten Sie folgende Fragen:

1. Welches MQTT-Topic abonniert Telegraf?
2. Welches InfluxDB-Bucket werden die Daten geschrieben?
3. Welches Flux-Query verwendet das Grafana-Dashboard, um die Daten abzurufen?

---

## Schritt 3: InfluxDB erkunden

1. Öffnen Sie InfluxDB über den weitergeleiteten Port `38086` im **Ports**-Panel und melden Sie sich an.
2. Navigieren Sie zu **Data Explorer**.
3. Suchen Sie nach dem Bucket `influxdb` und dem Measurement `temperature`.
4. Vergewissern Sie sich, dass Daten mit dem Feld `Message` (Temperaturwert) ankommen.

---

## Schritt 4: Grafana-Dashboard erkunden

1. Öffnen Sie Grafana über den weitergeleiteten Port `3000` im **Ports**-Panel und melden Sie sich an.
2. Navigieren Sie zu **Dashboards → IoT Temperature Dashboard**.
3. Das Panel zeigt die Durchschnittstemperatur der letzten 15 Minuten in 10-Sekunden-Intervallen.
4. Beobachten Sie, wie sich die simulierte Temperatur im Laufe der Zeit verändert (das Gerät wechselt zwischen Heiz- und Kühlmodus).

---

## Schritt 5: Eigenes Panel erstellen

Erstellen Sie in Grafana ein zweites Panel, das den **aktuellen (letzten) Temperaturwert** als Gauge (Tachometer) anzeigt.

Hinweise:

- Fügen Sie ein neues Panel vom Typ **Gauge** hinzu.
- Verwenden Sie folgendes Flux-Query als Ausgangspunkt:

```flux
from(bucket: "CurrentData")
  |> range(start: -1m)
  |> filter(fn: (r) => r["_measurement"] == "temperature")
  |> filter(fn: (r) => r["_field"] == "Message")
  |> last()
```

- Stellen Sie die Einheit auf **Celsius (°C)** ein.
- Definieren Sie sinnvolle Schwellenwerte (z. B. grün bis 22 °C, gelb bis 28 °C, rot ab 28 °C).

---

## Schritt 6: Telegraf-Konfiguration anpassen

Öffnen Sie `.devcontainer/telegraf.conf` und entfernen Sie `name_override`, sodass Telegraf den Topic-Namen als Measurement-Namen verwendet (statt immer `temperature`).

Starten Sie den Telegraf-Container neu, damit die Änderung wirksam wird:

```bash
docker restart telegraf
```

Beobachten Sie in InfluxDB, unter welchem Measurement-Namen die Daten nun gespeichert werden.

> **Hinweis:** Ohne `name_override` verwendet Telegraf den MQTT-Topic-Namen als Measurement-Namen. Achten Sie darauf, dass das Grafana-Query entsprechend angepasst wird.

---

## Bonus: Alert in Grafana einrichten

Konfigurieren Sie in Grafana einen **Alert**, der ausgelöst wird, wenn die Temperatur über **27 °C** steigt.

- Navigieren Sie im Panel zu **Alert → New alert rule**.
- Definieren Sie die Bedingung basierend auf dem Flux-Query aus Schritt 5.
- Wählen Sie als Notification-Channel die Option **Grafana managed alert**.
