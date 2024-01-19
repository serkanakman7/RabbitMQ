using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();

factory.UserName = "guest";
factory.Password = "guest";
factory.Port = 5672;
factory.HostName = "localhost";

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

var exchangeName = "topic-exchange-example";

channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true, false);

for(int i = 0; i < 100; i++)
{
    Console.Write("Mesajın gönderileceği topic formatını belirtiniz : ");
    string topic = Console.ReadLine()
        ;
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");

    channel.BasicPublish(exchangeName, topic, body: message);
}

    Console.WriteLine("Mesaj gönderildi");

Console.ReadLine();
