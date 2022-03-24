using DecentIOT.RabbitMQ.Client.Requester;
using DecentIOT.RabbitMQ.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace DecentIOT.RabbitMQ.Client.Responser
{
    public class RabbitResponse:RabbitMessage
    {
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
