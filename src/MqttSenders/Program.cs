using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MqttSenders;

var builder = Host.CreateApplicationBuilder(args);

var senders = builder.Configuration.GetSection("senders").Get<IEnumerable<string>>()
    ?? throw new InvalidOperationException("Could not find senders entry in configuration");

if (!senders.Any())
{
    throw new InvalidOperationException("senders entry found in configuration but has no values");
}

if (senders.Any(sender => sender.Equals("MQTT Simulator")))
{
    builder.Services.AddHostedService<MqttDeviceSimulator>();
}

if (senders.Any(sender => sender.Equals("OPC UA")))
{
    builder.Services.AddHostedService<Opc2MqttConverter>();
}

var host = builder.Build();
await host.RunAsync();