using DecentIOT.RabbitMQ.Producer;
using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Queue;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using DecentIOT.RabbitMQ.Consumer;
using DecentIOT.RabbitMQ.Client.Miscellaneous;
using DecentIOT.RabbitMQ.Message;
using DecentIOT.RabbitMQ.Client.Requester;

namespace DecentIOT.RabbitMQ.Client.Models.Responser
{
    public class RabbitResponser
    {
        public event Action<RabbitRequest, RabbitProducer> NewRequest;

        private IModel Channel { get; }
        private RabbitDirectExchange Exchange { get; }
        private RabbitConsumer Consumer { get; }
        private RabbitProducer Producer { get; }
        private RabbitQueue RequestQueue { get; }

        public string RequestKey { get; set; }

        public RabbitResponser(IModel channel, string exchange, string requestKey=null)
        {
            Channel = channel;
            Exchange = new RabbitDirectExchange(channel, exchange);
            RequestKey = requestKey;
            if(string.IsNullOrEmpty(RequestKey)) RequestKey = Generator.GenerateShortUid();
            RequestQueue = Exchange.CreateQueue($"request.queue.{exchange}", RequestKey);
            Consumer = new RabbitConsumer(channel, RequestQueue);
            Consumer.NewMessage += Consumer_NewMessage;
            Producer = new RabbitProducer(channel, exchange);
        }
        public void Listen()
        {
            Consumer.StartConsuming();
        }
        public RabbitRequest GetRequest()
        {
            try
            {
                var message = Consumer.PullMessage(RequestQueue);
                return new RabbitRequest(message);
            }
            catch (Exception)
            {

                return null;
            }
        }
        public void Respond(RabbitMessage response)
        {
            Producer.PublishSingle(response);
        }
        private void Consumer_NewMessage(RabbitConsumer consumer, string queue, RabbitMessage message)
        {
            if (message.RoutingKey.Equals(RequestKey))
            {
                var request = message as RabbitRequest;
                NewRequest?.Invoke(request, Producer);
            }
        }
    }
}
