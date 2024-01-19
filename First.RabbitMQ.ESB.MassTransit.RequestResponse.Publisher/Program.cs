using MassTransit;
using Shared.RequestResponseMessage;

string queueName = "request-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(configurator =>
{
    configurator.Host("localhost", 5672, "/", configure =>
    {
        configure.Username("guest");
        configure.Password("guest");
    });
});

await bus.StartAsync();

var request = bus.CreateRequestClient<RequestMessage>(new Uri($"rabbitmq://localhost/{queueName}"));

int i = 1;

while (i < 10)
{
    await Task.Delay(200);

    var response = await request.GetResponse<ResponseMessage>(new RequestMessage { MessageNo = i++, Text = $"{i}. request" });

    Console.WriteLine($"Response Received : {response.Message.Text}");
}
Console.Read();