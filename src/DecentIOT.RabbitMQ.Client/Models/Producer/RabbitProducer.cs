using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Message;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace DecentIOT.RabbitMQ.Producer
{
    public class RabbitProducer
    {
        private IModel Channel { get; }
        private string Exchange { get; }

        private RabbitProducer(IModel channel)
        {
            Channel = channel;
        }
        public RabbitProducer(IModel channel, string exchange)
            :this(channel)
        {
            Exchange = exchange;
        }
        public RabbitProducer(IModel channel, RabbitExchange exchange)
            : this(channel)
        {
            Exchange = exchange.Name;
        }


        public void PublishSingle(RabbitMessage message)
        {
            try
            {
                IBasicProperties properties = null;
                if (message.Headers != null)
                {
                    properties = Channel.CreateBasicProperties();
                    properties.Headers = message.Headers;
                }
                Channel.BasicPublish(Exchange, message.RoutingKey, properties, message.Encode());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void PublishMany(List<RabbitMessage> batch,bool mandatory = false)
        {
            try
            {
                var messages = Channel.CreateBasicPublishBatch();
                foreach (var m in batch)
                {
                    IBasicProperties properties = null;
                    if (m.Headers != null) Channel.CreateBasicProperties().Headers = m.Headers;
                    messages.Add(Exchange, m.RoutingKey, mandatory, properties, m.Encode());
                }
                messages.Publish();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
