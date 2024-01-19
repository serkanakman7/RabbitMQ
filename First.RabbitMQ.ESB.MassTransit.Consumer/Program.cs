using First.RabbitMQ.ESB.MassTransit.Consumer.Consumer;
using MassTransit;

string queueName = "esb-example-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host("localhost", 5672, "/", configure =>
    {
        configure.Username("guest");
        configure.Password("guest");
    });

    factory.ReceiveEndpoint(queueName, endpoint =>
    {
        endpoint.Consumer<ExampleMessageConsumer>();
    });
});

await bus.StartAsync();

Console.Read();

