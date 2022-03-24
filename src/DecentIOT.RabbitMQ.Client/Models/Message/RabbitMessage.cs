using Newtonsoft.Json;
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
        public string Content { get; private set; }
        public Type Type { get; private set; }
        public Dictionary<string,object> Headers { get; private set; }

        public RabbitMessage(string routingKey)
        {
            Id = Guid.NewGuid().ToString();
            RoutingKey = routingKey;
            Timestamp = DateTime.Now;
        }
        public RabbitMessage(string routingKey, string content)
        {
            Id = Guid.NewGuid().ToString();
            RoutingKey = routingKey;
            Timestamp = DateTime.Now;
            StoreContent(content);
        }

        [JsonConstructor]
        public RabbitMessage(string id, string routingKey, DateTime timestamp,string content, Type type)
        {
            Id = id;
            RoutingKey = routingKey;
            Timestamp = timestamp;
            Content = content;
            Type = type;
        }
        public void StoreContent<T>(T content)
        {
            try
            {
                Content = JsonConvert.SerializeObject(content);
                Type = typeof(T);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public T OpenContent<T>()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(Content);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public object OpenContent()
        {
            try
            {
                return JsonConvert.DeserializeObject(Content, Type);
            }
            catch (Exception)
            {

                throw;
            }
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
