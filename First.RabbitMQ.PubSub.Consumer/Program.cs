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

string exchangeName = "example-pub-sub-exchange";

channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);

string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queueName, exchangeName, string.Empty);

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(queueName, false, consumer);

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));
};
Console.Read();
