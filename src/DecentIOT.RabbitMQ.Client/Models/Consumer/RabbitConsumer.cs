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
        /// <summary>
        /// New Message on Queue, <Consumer,Queue Name,Message>
        /// </summary>
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
        /// <summary>
        /// Starts to listen to new messages on the provided queue. <see cref="NewMessage"/> event is fired on new message.
        /// </summary>
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
        /// <summary>
        /// Starts to listen to new messages on the provided queue. Only your custom event is fired on new message.
        /// </summary>
        /// <param name="newMessageEvent">Your custom new message event</param>
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
        /// <summary>
        /// Synchronously gets the message from queue, this dequeues the message, so only use it if not using the <see cref="StartConsuming"/> method.
        /// </summary>
        public RabbitMessage PullMessage()
        {
            try
            {
                return RabbitMessage.Decode(Channel.BasicGet(Queue, true).Body);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
