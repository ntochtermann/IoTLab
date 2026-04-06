namespace Interfaces
{
    public record IotMessage<T>(T Message, DateTimeOffset Timestamp, string Type);

    public record ProducedMessage(DateTimeOffset Timestamp, string ProductCode, int Quantity)
    {
        public string Name => "Produced";
    }
}
