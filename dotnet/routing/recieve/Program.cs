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

        chnl.QueueBind(q1.QueueName, exchange: exchange_name, routingKey: "Info");
        chnl.QueueBind(q1.QueueName, exchange: exchange_name, routingKey: "Warning");

        var consumer = new EventingBasicConsumer(chnl);

        consumer.Received += msgConsumer;

        chnl.BasicConsume(q1.QueueName, true, consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();

    }

    static void msgConsumer(object model, BasicDeliverEventArgs ea)
    {
        byte[] body = ea.Body.ToArray();
        string msg = Encoding.UTF8.GetString(body);

        Console.WriteLine("Message received");
        Console.WriteLine(msg);
    }
}
