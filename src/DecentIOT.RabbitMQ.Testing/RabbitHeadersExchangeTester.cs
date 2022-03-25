using DecentIOT.RabbitMQ.Message;
using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Queue;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DecentIOT.RabbitMQ.Consumer;
using static DecentIOT.RabbitMQ.Exchange.RabbitHeadersExchange;

namespace DecentIOT.RabbitMQ.Testing
{
    [TestClass]
    public class RabbitHeadersExchangeTester:RabbitTester
    {
        private RabbitHeadersExchange? HeadersExchange { get; set; }
        private List<RabbitQueue?> HeadersQueue { get; set; }

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
        public void T01_PublishTo_RabbitHeadersExchange_Pass()
        {
            Create_CreateHeadersExchange_Pass();
            var producer = Client?.CreateProducer(HeadersExchange);
            var message = new RabbitMessage("", "quack!");
            message.AddHeader("color","white");
            producer?.PublishSingle(message);

            Assert.IsNotNull(producer);
        }
        [TestMethod]
        public void T02_ConsumeFrom_RabbitHeadersExchange_Pass()
        {
            Create_CreateHeadersExchange_Pass();

            var consumers = new List<RabbitConsumer>();
            HeadersQueue.ForEach(f => consumers.Add(Client.CreateConsumer(f)));

            var messages = new List<RabbitMessage>();

            consumers.ForEach(c => messages.Add(c.PullMessage()));

            Assert.IsNotNull(messages.Count == consumers.Count);
        }
    }
}
