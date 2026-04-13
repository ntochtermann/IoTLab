using System.Net;
using System.Text.Json;
using Azure.Messaging.EventHubs;
using Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class EventHubThresholdLoggerFunction
    {
        private readonly ILogger<EventHubThresholdLoggerFunction> _logger;

        public EventHubThresholdLoggerFunction(
            ILogger<EventHubThresholdLoggerFunction> logger
        )
        {
            _logger = logger;
        }

        // TODO: implement the function trigger and log the message if the threshold is breached
    }
}
