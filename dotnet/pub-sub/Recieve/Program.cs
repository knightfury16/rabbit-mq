using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Recieve;

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

        var q1 = chnl.QueueDeclare();
        
        chnl.QueueBind(q1.QueueName,"logs", string.Empty);

        var consumer = new EventingBasicConsumer(chnl);

        consumer.Received += msgConsumer;

        chnl.BasicConsume(q1.QueueName, true, consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();

    }

    static void msgConsumer(object model, BasicDeliverEventArgs ea )
    {
        byte[] body = ea.Body.ToArray();
        string msg = Encoding.UTF8.GetString(body);

        Console.WriteLine("Message received");
        Console.WriteLine(msg);
    }
}
