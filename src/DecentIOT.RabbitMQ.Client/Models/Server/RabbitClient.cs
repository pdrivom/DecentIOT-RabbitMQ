using DecentIOT.RabbitMQ.Responder;
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
        /// <summary>
        /// A direct exchange delivers messages to queues based on the message routing key.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="durable"></param>
        /// <param name="autodelete"></param>
        /// <param name="arguments"></param>
        public RabbitDirectExchange CreateDirectExchange(string name, bool durable = true, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            return new RabbitDirectExchange(Channel, name, durable, autodelete, arguments);
        }
        /// <summary>
        /// A fanout exchange routes messages to all of the queues that are bound to it and the routing key is ignored.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="durable"></param>
        /// <param name="autodelete"></param>
        /// <param name="arguments"></param>
        public RabbitFanoutExchange CreateFanoutExchange(string name, bool durable = true, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            return new RabbitFanoutExchange(Channel, name, durable, autodelete, arguments);
        }
        /// <summary>
        /// A headers exchange is designed for routing on multiple attributes that are more easily expressed as message headers than a routing key.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="durable"></param>
        /// <param name="autodelete"></param>
        /// <param name="arguments"></param>
        public RabbitHeadersExchange CreateHeadersExchange(string name, bool durable = true, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            return new RabbitHeadersExchange(Channel, name, durable, autodelete, arguments);
        }
        /// <summary>
        /// Topic exchanges route messages to one or many queues based on matching between a message routing key and the pattern that was used to bind a queue to an exchange.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="durable"></param>
        /// <param name="autodelete"></param>
        /// <param name="arguments"></param>
        public RabbitTopicExchange CreateTopicExchange(string name,bool durable = true, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            return new RabbitTopicExchange(Channel, name,durable,autodelete,arguments);
        }
        /// <summary>
        /// A producer is a user integration that sends messages to an Exchange. Then Exchanges routes the messages to the Queues.
        /// </summary>
        /// <param name="exchange"></param>
        public RabbitProducer CreateProducer(RabbitExchange exchange)
        {
            return new RabbitProducer(Channel, exchange);
        }
        /// <summary>
        /// A consumer is a user integration that receives messages. In this abstraction each <see cref="RabbitConsumer"/> object consumes is binded to one Queue.
        /// </summary>
        /// <param name="queue"></param>
        public RabbitConsumer CreateConsumer(RabbitQueue queue)
        {
            return new RabbitConsumer(Channel, queue);
        }
        /// <summary>
        /// This is the Requester on the abstraction of the Request-Response pattern using queues. All the creation of the exchange/queues is handled.
        /// </summary>
        /// <param name="exchange">The name of the Direct Exchange to be created or of existing one.The adjacent <see cref="RabbitResponder"/> must share the same exchange.</param>
        /// <param name="requestKey">The key to be used when making requests. The adjacent <see cref="RabbitResponder"/> must share the same key.</param>
        public RabbitRequester CreateRequester(string exchange, string requestKey)
        {
            return new RabbitRequester(Channel, exchange, requestKey);
        }
        /// <summary>
        /// This is the Responder on the abstraction of the Request-Response pattern using queues. All the creation of the excahnge/queues is handled.
        /// </summary>
        /// <param name="exchange">The name of the Direct Exchange to be created or of existing one.The adjacent <see cref="RabbitRequester"/> must share the same exchange.</param>
        /// <param name="requestKey">The key to be used when receiving requests. The adjacent <see cref="RabbitRequester"/> must share the same key.</param>
        public RabbitResponder CreateResponser(string exchange, string requestKey)
        {
            return new RabbitResponder(Channel, exchange, requestKey);
        }
    }
}
