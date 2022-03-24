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
            base(channel, name, durable, autodelete)
        {
            Channel?.ExchangeDeclare(name, "fanout", durable, autodelete, arguments);
        }

        public RabbitQueue CreateQueue(string name, bool durable = true, bool exclusive = false, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            var queue = new RabbitQueue(Channel, name, durable, exclusive, autodelete, arguments);
            Channel.QueueBind(queue.Name, Name,"", arguments);
            return queue;
        }

        public RabbitQueue CreateQueue(bool durable = true, bool exclusive = false, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            var queue = new RabbitQueue(Channel, durable, exclusive, autodelete, arguments);
            Channel.QueueBind(queue.Name, Name, "", arguments);
            return queue;
        }
    }
}
