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

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(requestQueueName, true, consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);

    byte[] byteMessage = Encoding.UTF8.GetBytes($"İşlem tamamlandı {message}");
    IBasicProperties properties = e.BasicProperties;
    IBasicProperties replyProperties = channel.CreateBasicProperties();

    replyProperties.CorrelationId = properties.CorrelationId;

    channel.BasicPublish(string.Empty, properties.ReplyTo, replyProperties, byteMessage);
};
Console.Read();
