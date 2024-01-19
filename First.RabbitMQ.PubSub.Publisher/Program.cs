using RabbitMQ.Client;
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

for(int i = 0; i < 10; i++)
{
    byte[] message = Encoding.UTF8.GetBytes($"Message {i}");

    channel.BasicPublish(exchangeName, string.Empty, body: message);
}

Console.Read();
