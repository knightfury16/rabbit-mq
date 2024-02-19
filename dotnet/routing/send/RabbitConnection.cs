using RabbitMQ.Client;

namespace send
{
    public class RabbitConnection
    {
        private static RabbitConnection _instance;
        private readonly IConnection connection; 

        private RabbitConnection()
        {
            var factory = new ConnectionFactory{ HostName = "localhost", UserName = "admin", Password = "admin" };
            connection = factory.CreateConnection();
        }

        public static RabbitConnection Instance
        {
            get
            {
                _instance ??= new RabbitConnection();
                return _instance;
            }
        }

        public IConnection GetConnection()
        {
            return connection;
        }

    }
}
