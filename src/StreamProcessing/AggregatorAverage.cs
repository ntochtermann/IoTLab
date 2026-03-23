using Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace StreamProcessing
{
    public class AggregatorAverage : IStreamProcessingOperation<double, IotMessage<double>?>
    {
        private readonly ILogger<AggregatorAverage> logger;

        public AggregatorAverage(ILogger<AggregatorAverage> logger)
        {
            this.logger = logger;
        }

        public IotMessage<double>? HandleMessage(IotMessage<double> message)
        {
            // TODO: Implement the logic to calculate the average based on the instructions in Übung2.md.
            throw new NotImplementedException();
        }
    }
}
