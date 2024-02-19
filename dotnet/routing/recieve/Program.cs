using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace recieve;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory();

        factory.HostName = "localhost";
        factory.UserName = "admin";
        factory.Password = "admin";

        var cnn = factory.CreateConnection();

        var chnl = cnn.CreateModel();

        string exchange_name = "direct_logs";

        chnl.ExchangeDeclare(exchange_name, ExchangeType.Direct);

        var q1 = chnl.QueueDeclare();

        string routing_key = GetRotutingKey(args) ?? "Info";

        chnl.QueueBind(q1.QueueName, exchange: exchange_name, routingKey: routing_key);

        var consumer = new EventingBasicConsumer(chnl);

        consumer.Received += MsgConsumer;

        chnl.BasicConsume(q1.QueueName, true, consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();

    }

    private static string? GetRotutingKey(string[] args)
    {
        if (args.Length > 0)
        {
            string key = args[0].Trim();
            return key;
        }
        else return null;
    }

    static void MsgConsumer(object model, BasicDeliverEventArgs ea)
    {
        byte[] body = ea.Body.ToArray();
        string msg = Encoding.UTF8.GetString(body);

        Console.WriteLine("Message received");
        Console.WriteLine(msg);
    }
}
