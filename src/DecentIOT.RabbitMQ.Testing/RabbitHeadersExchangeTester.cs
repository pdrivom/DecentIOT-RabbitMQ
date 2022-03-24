using DecentIOT.RabbitMQ.Client.Message;
using DecentIOT.RabbitMQ.Producer;
using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Message;
using DecentIOT.RabbitMQ.Queue;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DecentIOT.RabbitMQ.Exchange.RabbitHeadersExchange;
using DecentIOT.RabbitMQ.Consumer;

namespace DecentIOT.RabbitMQ.Testing
{
    [TestClass]
    public class RabbitHeadersExchangeTester:RabbitTester
    {
        private RabbitHeadersExchange? HeadersExchange { get; set; }
        private List<RabbitQueue?> HeadersQueue { get; set; }

        [TestMethod]
        public void Create_CreateHeadersExchange_Pass()
        {
            Create_RabbitVitualServerClient_Pass();
            HeadersQueue = new List<RabbitQueue?>();
            HeadersExchange = Client?.CreateHeadersExchange("ex.headers");
            HeadersQueue.Add(HeadersExchange?.CreateQueue("headers.animal.queue1",x_match.any,new List<RabitMessageHeader> { new RabitMessageHeader("animal","duck"),new RabitMessageHeader("color","white")}));
            HeadersQueue.Add(HeadersExchange?.CreateQueue("headers.animal.queue2",x_match.any,new List<RabitMessageHeader> { new RabitMessageHeader("animal", "dog"), new RabitMessageHeader("color", "white") }));
            Assert.IsNotNull(HeadersQueue?.Count>0);
        }
        [TestMethod]
        public void PublishTo_RabbitHeadersExchange_Pass()
        {
            Create_CreateHeadersExchange_Pass();
            var producer = Client?.CreateProducer(HeadersExchange);
            var message = new RabbitMessage("", "quack!");
            message.AddHeader("color","white");
            producer?.PublishSingle(message);

            Assert.IsNotNull(producer);
        }
        [TestMethod]
        public void ConsumeFrom_RabbitHeadersExchange_Pass()
        {
            Create_CreateHeadersExchange_Pass();

            var consumers = new List<RabbitConsumer>();
            HeadersQueue.ForEach(f => consumers.Add(Client.CreateConsumer(f)));

            var messages = new List<RabbitMessage>();

            consumers.ForEach(c => messages.Add(c.PullMessage(c.Queue)));

            Assert.IsNotNull(messages.Count == consumers.Count);
        }
    }
}
