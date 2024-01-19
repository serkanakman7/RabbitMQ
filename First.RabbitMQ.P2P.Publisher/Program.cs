using RabbitMQ.Client;
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

byte[] message = Encoding.UTF8.GetBytes("Merhaba");

channel.BasicPublish(string.Empty, queueName, body: message);

Console.Read();
