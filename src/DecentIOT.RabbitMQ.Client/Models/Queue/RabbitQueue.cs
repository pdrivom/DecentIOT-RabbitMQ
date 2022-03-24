using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecentIOT.RabbitMQ.Queue
{
    public class RabbitQueue
    {
        public QueueDeclareOk QueueDeclare { get; }
        public string Name { get; }
        public bool Durable { get; }
        public bool Exclusive { get; }
        public bool Autodelete { get; }
        public IDictionary<string, object> Arguments { get; }

        private IModel Channel { get; set; }

        public RabbitQueue(IModel channel, string name, bool durable = true, bool exclusive = false, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            Channel = channel;
            Name = name;
            Durable = durable;
            Exclusive = exclusive;
            Autodelete = autodelete;
            Arguments = arguments;
            QueueDeclare = Channel.QueueDeclare(name, durable, exclusive, autodelete, null);
        }

        public RabbitQueue(IModel channel, bool durable = true, bool exclusive = false, bool autodelete = false, IDictionary<string, object> arguments = null)
            :this(channel,Guid.NewGuid().ToString(), durable,exclusive,autodelete,arguments)
        {
        }
    }
}
