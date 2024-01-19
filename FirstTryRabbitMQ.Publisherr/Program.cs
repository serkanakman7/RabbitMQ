using RabbitMQ.Client;
using System.Text;

namespace FirstTryRabbitMQ.Publisherr
{
    public enum LogNames
    {
        Critical=1,
        Info=2,
        Warning=3,
        Error=4
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port=5672,
                UserName ="guest",
                Password="guest"
            };

            //factory.Uri = new Uri("localhost:15672");  //amqps://twbeotmy:dn8sclVxFPfw7hUa4uhSo-oaKWPWf1S2@possum.lmq.cloudamqp.com/twbeotmy

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            //channel.QueueDeclare("hello-queue", true, false, false);

            channel.ExchangeDeclare("header-exchange", type: ExchangeType.Headers, durable: true);

            Dictionary<string,object> headers = new Dictionary<string, object>();

            headers.Add("format", "pdf");
            headers.Add("shape", "a4");

            var properties = channel.CreateBasicProperties();
            properties.Headers = headers;

            channel.BasicPublish("header-exchange", string.Empty,properties,Encoding.UTF8.GetBytes("Header mesajım"));

            Console.WriteLine("Mesaj gönderilmiş");

            Console.ReadLine();
        }
    }
}