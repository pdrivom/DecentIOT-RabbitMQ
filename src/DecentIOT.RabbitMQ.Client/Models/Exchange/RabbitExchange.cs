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
        public RabbitExchange(IModel channel, string name = "ex.topic")
        {
            Channel = channel;
            Name = name;
        }
        public void AddQueue(RabbitQueue queue,string routingKey)
        {
            Channel.QueueBind(queue.Name, Name, routingKey);
        }
        
    }
}
