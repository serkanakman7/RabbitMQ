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

var exchangeName = "topic-exchange-example";

channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true, false);

Console.Write("Dinlenecek topic formatını burda belirtiniz : ");
var topic = Console.ReadLine();

string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queueName, exchangeName,topic);

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(queueName, false, consumer);

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();