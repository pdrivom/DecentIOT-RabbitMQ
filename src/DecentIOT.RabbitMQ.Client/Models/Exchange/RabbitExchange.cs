using DecentIOT.RabbitMQ.Queue;
using RabbitMQ.Client;
using System.Collections.Generic;

namespace DecentIOT.RabbitMQ.Exchange
{
    public class RabbitExchange
    {
        internal IModel Channel { get; set; }
        /// <summary>
        /// Exchange name.
        /// </summary>
        public string Name { get; private set; }
        public RabbitExchange(IModel channel, string name = "ex.topic", bool durable = true, bool autodelete = false, IDictionary<string, object> arguments = null)
        {
            Channel = channel;
            Name = name;
        }
        
    }
}
