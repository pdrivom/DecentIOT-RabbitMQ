using System;
using System.Collections.Generic;
using System.Text;

namespace DecentIOT.RabbitMQ.Message
{
    public class RabitMessageHeader
    {
        public string Key { get; }
        public object Value { get; }

        public RabitMessageHeader(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}
