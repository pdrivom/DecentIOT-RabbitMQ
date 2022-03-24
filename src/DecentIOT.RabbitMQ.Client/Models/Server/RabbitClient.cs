using DecentIOT.RabbitMQ.Client.Models.Responser;
using DecentIOT.RabbitMQ.Consumer;
using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Producer;
using DecentIOT.RabbitMQ.Queue;
using DecentIOT.RabbitMQ.Requester;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecentIOT.RabbitMQ
{
    public class RabbitClient
    {
        public bool? Connected { get => Connection?.IsOpen; }

        private IModel Channel { get; }
        private ConnectionFactory Factory { get; }
        private IConnection Connection { get;  }


        private RabbitClient()
        {
            Factory = new ConnectionFactory();
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();
        }
        public RabbitClient(string hostname = "localhost", string virtualhost = "/" ,int port = 5672, string username = "guest", string password = "guest")
            :this()
        {
            Factory.HostName = hostname;
            Factory.VirtualHost = virtualhost;
            Factory.Port = port;
            Factory.UserName = username;
            Factory.Password = password;
        }
        public RabbitClient(RabbitClientSettings settings)
            : this()
        {
            Factory.HostName = settings.HostName;
            Factory.VirtualHost = settings.VirtualHost;
            Factory.Port = settings.Port;
            Factory.UserName = settings.UserName;
            Factory.Password = settings.Password;
        }

        public RabbitDirectExchange CreateDirectExchange(string name, bool durable = true, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            return new RabbitDirectExchange(Channel, name, durable, autodelete, arguments);
        }
        public RabbitFanoutExchange CreateFanoutExchange(string name, bool durable = true, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            return new RabbitFanoutExchange(Channel, name, durable, autodelete, arguments);
        }
        public RabbitHeadersExchange CreateHeadersExchange(string name, bool durable = true, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            return new RabbitHeadersExchange(Channel, name, durable, autodelete, arguments);
        }
        public RabbitTopicExchange CreateTopicExchange(string name,bool durable = true, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            return new RabbitTopicExchange(Channel, name,durable,autodelete,arguments);
        }
        public RabbitTopicExchange CreateTopicExchange(bool durable = true, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            return new RabbitTopicExchange(Channel, Guid.NewGuid().ToString(), durable, autodelete, arguments);
        }
        public RabbitProducer CreateProducer(string exchange)
        {
            return new RabbitProducer(Channel, exchange);
        }
        public RabbitProducer CreateProducer(RabbitExchange exchange)
        {
            return new RabbitProducer(Channel, exchange);
        }
        public RabbitConsumer CreateConsumer(string queue)
        {
            return new RabbitConsumer(Channel, queue);
        }
        public RabbitConsumer CreateConsumer(RabbitQueue queue)
        {
            return new RabbitConsumer(Channel, queue);
        }
        public RabbitRequester CreateRequester(string exchange, string requestKey)
        {
            return new RabbitRequester(Channel, exchange, requestKey);
        }
        public RabbitResponser CreateResponser(string exchange, string requestKey)
        {
            return new RabbitResponser(Channel, exchange, requestKey);
        }
    }
}
