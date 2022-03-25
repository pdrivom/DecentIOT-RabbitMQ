using DecentIOT.RabbitMQ.Miscellaneous;
using DecentIOT.RabbitMQ.Requester;
using DecentIOT.RabbitMQ.Responder;
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
        public event Action<RabbitRequester, RabbitResponse> NewResponse;

        private RabbitDirectExchange Exchange { get; }
        private string RequestKey { get; }
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
        }
        /// <summary>
        /// Starts to listen to incoming response. <see cref="NewResponse"/> event is fired on a response.
        /// </summary>
        public void Listen()
        {
            Consumer.NewMessage += Consumer_NewMessage;
            Consumer.StartConsuming();
        }
        /// <summary>
        /// Starts to listen to incoming response.Only your custom event is fired on new response.
        /// </summary>
        /// <param name="newResponse">Your custom new request event</param>
        public void Listen(Action<RabbitRequester, RabbitResponse> newResponse)
        {
            Consumer.NewMessage += (c,q,m) =>
            {
                newResponse?.Invoke(this, new RabbitResponse(m));
            };
            Consumer.StartConsuming();
        }
        /// <summary>
        /// Synchronously gets the response, this dequeues the message, so only use it if not using the <see cref="Listen"/> method.
        /// </summary>
        public RabbitResponse GetResponse()
        {
            try
            {
                var response = Consumer.PullMessage();
                return new RabbitResponse(response);
            }
            catch (Exception)
            {

                return null;
            }
        }
        /// <summary>
        /// Sends a request to the responder.
        /// </summary>
        /// <param name="request">The request content</param>
        public void SendRequest(dynamic request)
        {
            var message = new RabbitMessage(RequestKey, request);
            message.AddHeader("responseKey", ResponseRoutingKey);
            Producer.PublishSingle(message);
        }
        private void Consumer_NewMessage(RabbitConsumer consumer, string queue, RabbitMessage message)
        {
            NewResponse?.Invoke(this, new RabbitResponse(message));
        }

    }
}
