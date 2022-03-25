using DecentIOT.RabbitMQ.Producer;
using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Queue;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using DecentIOT.RabbitMQ.Consumer;
using DecentIOT.RabbitMQ.Miscellaneous;
using DecentIOT.RabbitMQ.Message;
using DecentIOT.RabbitMQ.Requester;

namespace DecentIOT.RabbitMQ.Responder
{
    public class RabbitResponder
    {
        public event Action<RabbitRequest> NewRequest;

        private IModel Channel { get; }
        private RabbitDirectExchange Exchange { get; }
        private RabbitConsumer Consumer { get; }
        private RabbitProducer Producer { get; }
        private RabbitQueue RequestQueue { get; }
        private string RequestKey { get; set; }

        public RabbitResponder(IModel channel, string exchange, string requestKey=null)
        {
            Channel = channel;
            Exchange = new RabbitDirectExchange(channel, exchange);
            RequestKey = requestKey;
            if(string.IsNullOrEmpty(RequestKey)) RequestKey = Generator.GenerateShortUid();
            RequestQueue = Exchange.CreateQueue($"request.queue.{exchange}", RequestKey);
            Consumer = new RabbitConsumer(channel, RequestQueue);
            
            Producer = new RabbitProducer(channel, exchange);
        }
        /// <summary>
        /// Starts to listen to incoming requests. <see cref="NewRequest"/> event is fired on new request.
        /// </summary>
        public void Listen()
        {
            Consumer.NewMessage += Consumer_NewMessage;
            Consumer.StartConsuming();
        }
        /// <summary>
        /// Starts to listen to incoming requests. Only your custom event is fired on new request.
        /// </summary>
        /// <param name="newRequest">Your custom new request event</param>
        public void Listen(Action<RabbitRequest, RabbitProducer> newRequest)
        {
            Consumer.NewMessage += (c,q,m) =>
            {
                if (m.RoutingKey.Equals(RequestKey))
                {
                    var request = m as RabbitRequest;
                    newRequest?.Invoke(request, Producer);
                }
            };
            Consumer.StartConsuming();
        }
        /// <summary>
        /// Synchronously gets the request, this dequeues the message, so only use it if not using the <see cref="Listen"/> method.
        /// </summary>
        public RabbitRequest GetRequest()
        {
            try
            {
                var message = Consumer.PullMessage();
                return new RabbitRequest(message);
            }
            catch (Exception)
            {

                return null;
            }
        }
        /// <summary>
        /// Sends a response to the requester.
        /// </summary>
        /// <param name="request">The request received</param>
        /// <param name="responseContent">The response content to the request. All routing is handled automatically.</param>
        public void SendResponse(RabbitRequest request,dynamic responseContent)
        {
            var response = new RabbitMessage(request.ResponseKey, responseContent);
            response.AddHeader("request",request.Serialize());
            Producer.PublishSingle(response);
        }
        private void Consumer_NewMessage(RabbitConsumer consumer, string queue, RabbitMessage message)
        {
            if (message.RoutingKey.Equals(RequestKey))
            {
                var request = message as RabbitRequest;
                NewRequest?.Invoke(request);
            }
        }
    }
}
