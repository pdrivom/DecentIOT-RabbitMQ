using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecentIOT.RabbitMQ.Message
{
    public class RabbitMessage
    {
        public string Id { get; }
        public string RoutingKey { get; }
        public DateTime Timestamp { get; }
        public dynamic Content { get;  set; }
        public Type Type { get; private set; }
        public Dictionary<string,object> Headers { get; private set; }


        public RabbitMessage(string routingKey, dynamic content)
        {
            Id = Guid.NewGuid().ToString();
            RoutingKey = routingKey;
            Timestamp = DateTime.Now;
            Content = content;
            Type = ((object)Content).GetType();
        }

        [JsonConstructor]
        public RabbitMessage(string id, string routingKey, DateTime timestamp,dynamic content, Type type, Dictionary<string, object> headers)
        {
            Id = id;
            RoutingKey = routingKey;
            Timestamp = timestamp;
            Content = content;
            Type = type;
            Headers = headers;
        }

        public ReadOnlyMemory<byte> Encode()
        {
            try
            {
                return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static RabbitMessage Decode(ReadOnlyMemory<byte> payload)
        {
            try
            {
                string json = Encoding.UTF8.GetString(payload.ToArray());
                return JsonConvert.DeserializeObject<RabbitMessage>(json);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void AddHeader(string key, object value)
        {
            if (Headers == null) Headers = new Dictionary<string, object>();
            if (!Headers.ContainsKey(key)) Headers.Add(key, value);            
        }
    }
}
