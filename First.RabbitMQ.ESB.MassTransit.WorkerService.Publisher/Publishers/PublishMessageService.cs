using MassTransit;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First.RabbitMQ.ESB.MassTransit.WorkerService.Publisher.Publishers
{
    public class PublishMessageService : BackgroundService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public PublishMessageService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0;
            while (i<10)
            {
                ExampleMessage message = new ExampleMessage
                {
                    Text = $"{++i} Merhaba"
                };

                await _publishEndpoint.Publish(message);
            }
        }
    }
}
