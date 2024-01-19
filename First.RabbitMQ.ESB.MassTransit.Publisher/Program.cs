using MassTransit;
using Shared.Messages;

string queueName = "esb-example-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host("localhost", 5672, "/", configure =>
    {
        configure.Username("guest");
        configure.Password("guest");
    });
});

ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(new($"rabbitmq://localhost/{queueName}"));

Console.Write("Bir mesaj giriniz : ");
string message = Console.ReadLine();

await sendEndpoint.Send<IMessage>(new ExampleMessage
{
    Text = message
});

Console.Read();
