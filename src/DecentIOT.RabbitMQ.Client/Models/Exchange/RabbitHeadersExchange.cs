using DecentIOT.RabbitMQ.Client.Message;
using DecentIOT.RabbitMQ.Queue;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecentIOT.RabbitMQ.Exchange
{
    public class RabbitHeadersExchange : RabbitExchange
    {
        public enum x_match
        {
            any,
            all
        }

        public RabbitHeadersExchange(IModel channel, string name = "ex.headers", bool durable = true, bool autodelete = false, IDictionary<string, object> arguments = null) :
            base(channel, name, durable, autodelete)
        {
            Channel?.ExchangeDeclare(name, "headers", durable, autodelete, arguments);
        }
        public RabbitQueue CreateQueue(string name, x_match match , IList<RabitMessageHeader> headers, bool durable = true, bool exclusive = false, bool autodelete = false)
        {
            var arguments = headers.ToDictionary(k => k.Key, v => v.Value);
            arguments.Add("x-match", match.ToString());
            var queue = new RabbitQueue(Channel, name, durable, exclusive, autodelete, arguments);
            Channel.QueueBind(queue.Name, Name, "", arguments);
            return queue;
        }

        public RabbitQueue CreateQueue(IDictionary<string, object> arguments,bool durable = true, bool exclusive = false, bool autodelete = false)
        {
            var queue = new RabbitQueue(Channel, durable, exclusive, autodelete, arguments);
            Channel.QueueBind(queue.Name, Name, "", arguments);
            return queue;
        }
    }
}