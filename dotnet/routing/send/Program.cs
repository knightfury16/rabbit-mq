using RabbitMQ.Client;
using System.Text;
using System.Xml.Schema;

namespace send;

class Program
{
    static void Main(string[] args)
    {

        var rabbitConnection = RabbitConnection.Instance;

        var connection = rabbitConnection.GetConnection();

        IModel chnl = connection.CreateModel();

        string exchange_name = "direct_logs";

        chnl.ExchangeDeclare(exchange_name, ExchangeType.Direct);

        Log log = GetMessage(args);

        var msg = Encoding.UTF8.GetBytes(log.ToString());

        chnl.BasicPublish(exchange: exchange_name, routingKey: log.Severity.ToString(), body: msg);

        Console.WriteLine($"{log} sent to queue");
        Console.ReadLine();
    }

    private static Log GetMessage(string[] args)
    {
        if (args.Length > 0)
        {
            if (Enum.TryParse(args[1], out Severity severity))
            {
                string message = args[2];
                return new Log(message, severity);
            }

            return new Log("Default", Severity.Info);
        }
        else  return new Log("default", Severity.Info); 
    }
}

public enum Severity
{
    Info,
    Warning,
    Error,
}

class Log
{
    public string Message { get; set; }
    public Severity Severity { get; set; }

    public Log(string message, Severity severity)
    {
        Message = message;
        Severity = severity;
    }

    public override string ToString()
    {
        return $"[{Severity}] {Message}";
    }
}
