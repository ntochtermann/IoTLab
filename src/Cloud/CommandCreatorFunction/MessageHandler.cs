using System.Text;
using System.Text.Json;
using Interfaces;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Configuration;

namespace CommandCreatorFunction;

public class MessageHandler
{
    private readonly ServiceClient serviceClient;
    private readonly string deviceId;

    public MessageHandler(ServiceClient serviceClient, IConfiguration config)
    {
        this.serviceClient = serviceClient;
        this.deviceId = config.GetValue<string>("IoTHubDeviceId") ?? throw new InvalidOperationException("No IoT Hub Device ID Found");
    }

    public async Task HandleMessageAsync(IotMessage<double> message)
    {
        if (message.Message > 24)
        {
            await SendColderMessage();
        }
        else if (message.Message < 18)
        {
            await SendWarmerMessage();
        }
    }

    private async Task SendColderMessage()
    {
        // use the ServiceClient to send a message indicating the iot device to decrease the temperature
    }

    private async Task SendWarmerMessage()
    {
        // use the ServiceClient to send a message indicating the iot device to increase the temperature
    }

    private static Message ConvertControlMessageToMessage(ControlMessage messageToConvert)
    {
        var jsonMessage = JsonSerializer.Serialize(messageToConvert);
        return new Message(Encoding.UTF8.GetBytes(jsonMessage));
    }
}
