using DecentIOT.RabbitMQ.Requester;
using DecentIOT.RabbitMQ.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace DecentIOT.RabbitMQ.Responder
{
    public class RabbitResponse:RabbitMessage
    {
        /// <summary>
        /// The request of this response.
        /// </summary>
        public RabbitMessage Request { get; }

        public RabbitResponse(RabbitMessage message)
            : this(message.RoutingKey, (object)message.Content, message.Headers["request"])
        {

        }

        public RabbitResponse(string responsekey, object response, object request):base(responsekey, response)
        {
            Request = Deserialize((string)request); 
        }
    }
}
