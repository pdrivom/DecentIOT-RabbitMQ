using DecentIOT.RabbitMQ.Consumer;
using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Message;
using DecentIOT.RabbitMQ.Queue;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DecentIOT.RabbitMQ.Testing
{
    [TestClass]
    public class RabbitFanoutExchangeTester : RabbitTester
    {

        private RabbitFanoutExchange? FanoutExchange { get; set; }
        private List<RabbitQueue>? FanoutQueues { get; set; }


        public void Create_CreateFanoutExchange_Pass()
        {
            
            Create_RabbitVitualServerClient_Pass();
            FanoutQueues = new List<RabbitQueue>();
            FanoutExchange = Client?.CreateFanoutExchange("ex.fanout");
            FanoutQueues?.Add(FanoutExchange.CreateQueue("fanout.animal.queue1"));
            FanoutQueues?.Add(FanoutExchange.CreateQueue("fanout.animal.queue2"));
            Assert.IsTrue(FanoutQueues?.Count > 0);
        }
        [TestMethod]
        public void T01_PublishTo_RabbitFanoutExchange_Pass()
        {
            Create_CreateFanoutExchange_Pass();
            var producer = Client?.CreateProducer(FanoutExchange);
            producer?.PublishSingle(new RabbitMessage("duck.image.png", "quack!"));
            Assert.IsNotNull(producer);
        }
        [TestMethod]
        public void T02_ConsumeFrom_RabbitFanoutExchange_Pass()
        {
            Create_CreateFanoutExchange_Pass();

            var consumers = new List<RabbitConsumer>();
            FanoutQueues.ForEach(f => consumers.Add(Client.CreateConsumer(f)));

            var messages = new List<RabbitMessage>();

            consumers.ForEach(c => messages.Add(c.PullMessage()));

            Assert.IsNotNull(messages.Count == consumers.Count);
        }
    }
}
