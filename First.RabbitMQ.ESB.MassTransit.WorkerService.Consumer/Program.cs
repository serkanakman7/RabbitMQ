using First.RabbitMQ.ESB.MassTransit.WorkerService.Consumer.Consumers;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(configurator =>
        {

            configurator.AddConsumer<ExampleMessageConsumer>();

            configurator.UsingRabbitMq((context, _configurator) =>
            {
                _configurator.Host("localhost", 5672, "/", configure =>
                {
                    configure.Username("guest");
                    configure.Password("guest");
                });

                _configurator.ReceiveEndpoint("esb-example-queue", endpoint =>
                {
                    endpoint.ConfigureConsumer<ExampleMessageConsumer>(context);
                });
            });
        });
    })
    .Build();

await host.RunAsync();
