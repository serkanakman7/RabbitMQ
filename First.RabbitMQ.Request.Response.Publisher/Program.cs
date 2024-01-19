using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.UserName = "guest";
factory.Password = "guest";
factory.Port = 5672;
factory.HostName = "localhost";

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string requestQueueName = "example-request-response-queue";
channel.QueueDeclare(requestQueueName, false, false, false);

string replyQueueName = channel.QueueDeclare().QueueName;

string correlationId = Guid.NewGuid().ToString();

IBasicProperties properties = channel.CreateBasicProperties();
properties.ReplyTo = replyQueueName;
properties.CorrelationId = correlationId;

for(int i =0; i < 10; i++)
{
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    channel.BasicPublish(string.Empty, requestQueueName, properties, message);


}

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(replyQueueName, true, consumer);

consumer.Received += (sender, e) =>
{
    if (e.BasicProperties.CorrelationId == correlationId)
        Console.WriteLine($"Response : {Encoding.UTF8.GetString(e.Body.Span)}");
};

Console.Read();

