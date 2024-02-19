using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;



namespace Send;



public class RpcClient : IDisposable
{
    private const string RPC_SERVER = "rpc_server";

    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly QueueDeclareOk rpl_queue;

    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper = new();
    public RpcClient()
    {
        var factory = new ConnectionFactory {HostName= "localhost", UserName = "admin", Password = "admin" };
        connection = factory.CreateConnection();

        channel = connection.CreateModel();

        rpl_queue = channel.QueueDeclare(queue: "", durable: false, exclusive: true, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                return;
            var body = ea.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);

            tcs.TrySetResult(response);
        };

        channel.BasicConsume(consumer: consumer,
            queue: rpl_queue.QueueName,
            autoAck: true);
    }


    public Task<string> CallAsync(string message, CancellationToken cancellationToken = default)
    {
        IBasicProperties props = channel.CreateBasicProperties();
        var correlationId = Guid.NewGuid().ToString();
        props.CorrelationId = correlationId;
        props.ReplyTo = rpl_queue.QueueName;
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var tcs = new TaskCompletionSource<string>();
        callbackMapper.TryAdd(correlationId, tcs);

        channel.BasicPublish(exchange: string.Empty,
                             routingKey: RPC_SERVER,
                             basicProperties: props,
                             body: messageBytes);

        cancellationToken.Register(() => callbackMapper.TryRemove(correlationId, out _));
        return tcs.Task;
    }

    public void Dispose()
    {
        connection.Close();
    }
}
