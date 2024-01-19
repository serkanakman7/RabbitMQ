using First.RabbitMQ.ESB.MassTransit.WorkerService.Publisher;
using First.RabbitMQ.ESB.MassTransit.WorkerService.Publisher.Publishers;
using MassTransit;
using MassTransit.Transports;
using IHost = Microsoft.Extensions.Hosting.IHost;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, _configurator) =>
            {
                _configurator.Host("localhost", 5672, "/", configure =>
                {
                    configure.Username("guest");
                    configure.Password("guest");
                });
            });
        });
         services.AddHostedService<PublishMessageService>(impFactory =>
             {
             using IServiceScope scope = impFactory.CreateScope();
             IPublishEndpoint publishEndpoint = scope.ServiceProvider.GetService<IPublishEndpoint>();

             return new(publishEndpoint);
         });
    })
    .Build();

host.Run();
