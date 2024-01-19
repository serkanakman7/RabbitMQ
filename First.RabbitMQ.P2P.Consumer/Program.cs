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

string queueName = "example-p2p-queue";

channel.QueueDeclare(queueName, false, false, false);

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(queueName, false, consumer);

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};
Console.Read();
