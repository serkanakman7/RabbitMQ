using First.RabbitMQ.ESB.MassTransit.RequestResponse.Consumer.Consumers;
using MassTransit;

string queueName = "request-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(configurator =>
{
    configurator.Host("localhost", 5672, "/", configure =>
    {
        configure.Username("guest");
        configure.Password("guest");
    });

    configurator.ReceiveEndpoint(queueName, endpoint =>
    {
        endpoint.Consumer<RequestMessageConsumer>();
    });
});
await bus.StartAsync();

Console.Read();
