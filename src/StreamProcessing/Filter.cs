using Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamProcessing
{
    public class Filter : IStreamProcessingOperation<double, bool>
    {
        private readonly ILogger<Filter> logger;

        public Filter(ILogger<Filter> logger)
        {
            this.logger = logger;
        }

        public bool HandleMessage(IotMessage<double> message)
        {
            // TODO: Implement the logic to filter messages based on the instructions in Übung2.md.
            if (message is null)
            {
                logger.LogInformation("Received null message");
            }

            var temp = (double)message.Message;

            if (temp >= 20)
            {
                logger.LogInformation("Message dismissed: {message}", message);
                return false;
            }

            logger.LogInformation("Message accepted: {message}", message);
            return true;
        }
    }
}
