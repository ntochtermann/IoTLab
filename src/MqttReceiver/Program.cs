using MqttReceiver;
using StreamProcessing;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<MessageReceiver>();
builder.Services.AddStreamProcessing();

var host = builder.Build();
host.Run();
