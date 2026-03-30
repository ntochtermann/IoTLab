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
            var acceptMessage = message.Message < 20;
            var logMessage = acceptMessage ? "Message accepted: {message}" : "Message dismissed: {message}";
            logger.LogInformation(logMessage, message.Message);
            return acceptMessage;
        }
    }
}
