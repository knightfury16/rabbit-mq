using RabbitMQ.Client;

namespace send
{
    internal class GetRabbitConnection
    {
        private static GetRabbitConnection _instance;
        private readonly IConnection connection; 

        private GetRabbitConnection()
        {
            var factory = new ConnectionFactory{ HostName = "localhost", UserName = "admin", Password = "admin" };
            connection = factory.CreateConnection();
        }

        public static GetRabbitConnection Instance
        {
            get
            {
                _instance ??= new GetRabbitConnection();
                return _instance;
            }
        }

        public IConnection GetConnection()
        {
            return connection;
        }

    }
}
