using Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace StreamProcessing
{
    public class AggregatorAverage : IStreamProcessingOperation<double, IotMessage<double>?>
    {
        private readonly ILogger<AggregatorAverage> logger;

        private Queue<IotMessage<double>> messageQueue;

        private const int count = 10;

        public AggregatorAverage(ILogger<AggregatorAverage> logger)
        {
            this.logger = logger;
            this.messageQueue = new Queue<IotMessage<double>>();
        }

        public IotMessage<double>? HandleMessage(IotMessage<double> message)
        {
            // TODO: Implement the logic to calculate the average based on the instructions in Übung2.md.
            messageQueue.Enqueue(message);
            logger.LogInformation("QueueLength: {length}", messageQueue.Count);

            if (messageQueue.Count == count)
            {
                double avg = messageQueue.Average(m => m.Message);
                messageQueue.Clear();

                return new IotMessage<double>(avg, DateTimeOffset.Now, "aggregate");
            }

            return null;
        }
    }
}
