# Übung 2: Stream Processing

In dieser Übung implementieren Sie zwei Klassen, die Teil eines Stream-Processing-Systems sind: `Filter` und `AggregatorAverage`. Beide Klassen verwenden generische Schnittstellen und arbeiten mit IoT-Nachrichten.

## Aufgabe 1: Implementierung der Klasse `Filter`

Die Klasse `Filter` soll Nachrichten basierend auf einem Schwellenwert filtern. Implementieren Sie die Methode `HandleMessage` so, dass:

1. Nachrichten akzeptiert werden, wenn ihr Wert kleiner als 20 ist.
2. Akzeptierte Nachrichten mit dem Log-Level `Information` protokolliert werden, z. B.: `"Message accepted: {message}"`.
3. Abgelehnte Nachrichten ebenfalls mit dem Log-Level `Information` protokolliert werden, z. B.: `"Message dismissed: {message}"`.

## Aufgabe 2: Implementierung der Klasse `AggregatorAverage`

Die Klasse `AggregatorAverage` soll den Durchschnittswert der letzten 10 Nachrichten berechnen. Implementieren Sie die Methode `HandleMessage` so, dass:

1. Jede eingehende Nachricht in eine Warteschlange (`Queue`) eingefügt wird.
2. Die aktuelle Länge der Warteschlange mit dem Log-Level `Information` protokolliert wird, z. B.: `"QueueLength: {length}"`.
3. Sobald die Warteschlange 10 Nachrichten enthält:
   - Der Durchschnittswert berechnet wird.
   - Die Warteschlange geleert wird.
   - Eine neue aggregierte Nachricht mit dem Durchschnittswert, dem aktuellen Zeitstempel und dem Typ `"aggregate"` zurückgegeben wird.
4. Wenn die Warteschlange weniger als 10 Nachrichten enthält, soll `null` zurückgegeben werden.

## Hinweise

- Verwenden Sie die bereitgestellten Schnittstellen und Konstruktoren.
- Nutzen Sie die Logging-Funktionalität (`ILogger`) für Protokollierungszwecke.
- Testen Sie Ihre Implementierung mit verschiedenen Eingabewerten, um sicherzustellen, dass sie korrekt funktioniert.
