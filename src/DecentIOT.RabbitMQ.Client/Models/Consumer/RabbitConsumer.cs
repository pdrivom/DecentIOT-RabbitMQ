using DecentIOT.RabbitMQ.Message;
using DecentIOT.RabbitMQ.Queue;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecentIOT.RabbitMQ.Consumer
{
    public class RabbitConsumer
    {
        public event Action<RabbitConsumer,string, RabbitMessage> NewMessage;


        public string Queue { get; }
        private IModel Channel { get; }
        private EventingBasicConsumer Consumer { get; }


        private RabbitConsumer(IModel channel)
        {
            Channel = channel;
            Consumer = new EventingBasicConsumer(Channel);
        }
        public RabbitConsumer(IModel channel,string queue)
            :this(channel)
        {
            Queue = queue;
        }
        public RabbitConsumer(IModel channel, RabbitQueue queue)
            : this(channel)
        {
            Queue = queue.Name;
        }
        public void StartConsuming()
        {
            try
            {
                Consumer.Received += (model, ea) =>
                {
                    NewMessage?.Invoke(this, Queue, RabbitMessage.Decode(ea.Body));
                };
                Channel.BasicConsume(Queue, true, Consumer);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void StartConsuming(Action<RabbitConsumer,string, RabbitMessage> newMessageEvent)
        {
            try
            {
                Consumer.Received += (model, ea) =>
                {
                    newMessageEvent?.Invoke(this, Queue, RabbitMessage.Decode(ea.Body));
                };
                Channel.BasicConsume(Queue, true, Consumer);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public RabbitMessage PullMessage(string queue)
        {
            try
            {
                return RabbitMessage.Decode(Channel.BasicGet(queue, true).Body);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public RabbitMessage PullMessage(RabbitQueue queue)
        {
            try
            {
                return RabbitMessage.Decode(Channel.BasicGet(queue.Name, true).Body);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
