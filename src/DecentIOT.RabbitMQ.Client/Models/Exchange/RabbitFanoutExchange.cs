using DecentIOT.RabbitMQ.Queue;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecentIOT.RabbitMQ.Exchange
{
    public class RabbitFanoutExchange:RabbitExchange
    {

        public RabbitFanoutExchange(IModel channel, string name = "ex.fanout", bool durable = true, bool autodelete = false, IDictionary<string, object> arguments = null) :
            base(channel, name)
        {
            Channel?.ExchangeDeclare(name, "fanout", durable, autodelete, arguments);
        }
        /// <summary>
        /// Add a queue and binds to current Exchange.
        /// </summary>
        /// <param name="name">Name of the queue</param>
        /// <param name="durable">The queue will survive a broker restart.</param>
        /// <param name="exclusive">Used by only one connection and the queue will be deleted when that connection closes.</param>
        /// <param name="autodelete">Queue that has had at least one consumer is deleted when last consumer unsubscribes.</param>
        /// <param name="arguments">Optional; used by plugins and broker-specific features such as message TTL, queue length limit, etc</param>
        public RabbitQueue CreateQueue(string name, bool durable = true, bool exclusive = false, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            var queue = new RabbitQueue(Channel, name, durable, exclusive, autodelete, arguments);
            Channel.QueueBind(queue.Name, Name,"", arguments);
            return queue;
        }
        /// <summary>
        /// Add a queue and binds to current Exchange. It's name is autogenerated.
        /// </summary>
        /// <param name="durable">The queue will survive a broker restart.</param>
        /// <param name="exclusive">Used by only one connection and the queue will be deleted when that connection closes.</param>
        /// <param name="autodelete">Queue that has had at least one consumer is deleted when last consumer unsubscribes.</param>
        /// <param name="arguments">Optional; used by plugins and broker-specific features such as message TTL, queue length limit, etc</param>
        public RabbitQueue CreateQueue(bool durable = true, bool exclusive = false, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            var queue = new RabbitQueue(Channel, durable, exclusive, autodelete, arguments);
            Channel.QueueBind(queue.Name, Name, "", arguments);
            return queue;
        }
    }
}
