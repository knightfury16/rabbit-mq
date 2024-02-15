using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Receive;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory();
        factory.HostName = "localhost";
        factory.UserName = "admin";
        factory.Password = "admin";

        var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        var queue = "hello";

        channel.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false, arguments:null);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
        };

        channel.BasicConsume(
            queue: queue,
            autoAck: true,
            consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}
