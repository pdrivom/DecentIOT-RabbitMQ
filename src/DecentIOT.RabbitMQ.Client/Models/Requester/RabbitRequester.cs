using DecentIOT.RabbitMQ.Client.Miscellaneous;
using DecentIOT.RabbitMQ.Client.Requester;
using DecentIOT.RabbitMQ.Client.Responser;
using DecentIOT.RabbitMQ.Consumer;
using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Message;
using DecentIOT.RabbitMQ.Producer;
using DecentIOT.RabbitMQ.Queue;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DecentIOT.RabbitMQ.Requester
{
    public class RabbitRequester
    {
        public event Action<RabbitRequester,RabbitMessage> Response;

        public RabbitDirectExchange Exchange { get; }
        public string RequestKey { get; }

        private IModel Channel { get; }
        private RabbitQueue ResponseQueue { get; }
        private string ResponseRoutingKey { get; }
        private RabbitProducer Producer { get; }
        private RabbitConsumer Consumer { get; }


        public RabbitRequester(IModel channel, string exchange,string requestKey, bool durable = true, bool autodelete = false)
        {
            Channel = channel;
            Exchange = new RabbitDirectExchange(Channel, exchange, durable, autodelete, null);
            ResponseRoutingKey = Generator.GenerateShortUid();
            RequestKey = requestKey;
            ResponseQueue = Exchange.CreateQueue($"response.queue.{exchange}", ResponseRoutingKey);
            Producer = new RabbitProducer(Channel, Exchange);
            Consumer = new RabbitConsumer(Channel, ResponseQueue);
            Consumer.NewMessage += Consumer_NewMessage;
        }
        public void Listen()
        {
            Consumer.StartConsuming();
        }
        public RabbitResponse GetResponse()
        {
            try
            {
                var response = Consumer.PullMessage(ResponseQueue);
                return new RabbitResponse(response);
            }
            catch (Exception)
            {

                return null;
            }
        }
        public void Request(dynamic request)
        {
            var message = new RabbitMessage(RequestKey, request);
            message.AddHeader("responseKey", ResponseRoutingKey);
            Producer.PublishSingle(message);
        }
        private void Consumer_NewMessage(RabbitConsumer consumer, string queue, RabbitMessage message)
        {
            Response?.Invoke(this, message);
        }

    }
}
