using Interfaces;
using Microsoft.Extensions.Logging;

namespace StreamProcessing
{
    public class AggregatorAverage : IStreamProcessingOperation<double, IotMessage<double>?>
    {
        private readonly ILogger<AggregatorAverage> logger;
        private Queue<double> Messages;
        private int QueueLength;

        public AggregatorAverage(ILogger<AggregatorAverage> logger)
        {
            Messages = new Queue<double>();
            QueueLength = 10;
            this.logger = logger;
        }

        public IotMessage<double>? HandleMessage(IotMessage<double> message)
        {
            Messages.Enqueue(message.Message);
            logger.LogInformation("Message added to queue: {message}", message.Message);
            logger.LogInformation("QueueLength: {length}", Messages.Count);
            if (Messages.Count >= QueueLength)
            {
                double average = Messages.Average();
                Messages.Clear();
                return new IotMessage<double>(average, DateTimeOffset.Now, "aggregate");
            }

            return null;
        }
    }
}
