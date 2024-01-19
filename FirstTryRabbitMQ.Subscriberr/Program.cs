using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FirstTryRabbitMQ.Subscriberr
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            //factory.Uri = new Uri("amqps://twbeotmy:dn8sclVxFPfw7hUa4uhSo-oaKWPWf1S2@possum.lmq.cloudamqp.com/twbeotmy");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.BasicQos(0, 1, false);

            var subscriber = new EventingBasicConsumer(channel);

            Dictionary<string,object> headers = new Dictionary<string, object>();

            headers.Add("format", "pdf");
            headers.Add("shape", "a4");
            headers.Add("x-match", "all");

            var randomQueueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(randomQueueName, "header-exchange", string.Empty, headers);

            channel.BasicConsume(randomQueueName, false, subscriber);

            Console.WriteLine("Loglar dinleniyor");

            subscriber.Received += (object? sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(1500);
                Console.WriteLine("Gelen Mesaj:"+message);

                File.AppendAllText("log-critical.txt", message);

                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();
        }
    }
}