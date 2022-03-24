﻿using DecentIOT.RabbitMQ.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace DecentIOT.RabbitMQ.Client.Requester
{
    public class RabbitRequest:RabbitMessage
    {
        public string ResponseKey { get; }

        public RabbitRequest(RabbitMessage message)
            : this(message.RoutingKey, (object)message.Content, message.Headers["responseKey"].ToString())
        {

        }

        public RabbitRequest(string requestkey,object request,string responseKey):base(requestkey, request)
        {
            ResponseKey = responseKey;
        }
    }
}