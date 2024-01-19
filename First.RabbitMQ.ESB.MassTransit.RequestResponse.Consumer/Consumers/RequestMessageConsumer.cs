using MassTransit;
using Shared.RequestResponseMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First.RabbitMQ.ESB.MassTransit.RequestResponse.Consumer.Consumers
{
    public class RequestMessageConsumer : IConsumer<RequestMessage>
    {
        public async Task Consume(ConsumeContext<RequestMessage> context)
        {
            Console.WriteLine(context.Message.Text);

            await context.RespondAsync<ResponseMessage>(new ResponseMessage { Text = $"{context.Message.MessageNo}. response to request" });
        }
    }
}
