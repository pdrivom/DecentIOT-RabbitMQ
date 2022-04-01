namespace DecentIOT.RabbitMQ
{
    public class RabbitClientSettings
    {
        public string HostName { get; private set; }
        public string VirtualHost { get; private set; }
        public int Port { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }

        public RabbitClientSettings(string hostname = "localhost", string virtualhost = "/", int port = 5672, string username = "guest", string password = "guest")
        {
            HostName = hostname;
            VirtualHost = virtualhost;
            Port = port;
            UserName = username;
            Password = password;
        }
    }
}
