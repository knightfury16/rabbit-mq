using RabbitMQ.Client;
using System.Text;

namespace Send;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory(); 
        factory.HostName = "localhost";
        factory.UserName = "admin";
        factory.Password = "admin";

        IConnection connection = factory.CreateConnection();
        IModel chnl = connection.CreateModel();

        chnl.ExchangeDeclare("logs", ExchangeType.Fanout);

        var msg =Encoding.UTF8.GetBytes(GetMessage(args));

        chnl.BasicPublish(exchange: "logs", routingKey: String.Empty, body: msg);

        Console.WriteLine("Message sent to queue");
        Console.ReadLine();
    }

    private static string GetMessage(string[] args)
    {
        return (args.Length > 0) ? args[1] : "Empty String";
    }
}
