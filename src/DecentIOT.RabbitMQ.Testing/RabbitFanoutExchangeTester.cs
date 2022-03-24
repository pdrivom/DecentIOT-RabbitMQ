using DecentIOT.RabbitMQ.Consumer;
using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Message;
using DecentIOT.RabbitMQ.Queue;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecentIOT.RabbitMQ.Testing
{
    [TestClass]
    public class RabbitFanoutExchangeTester : RabbitTester
    {

        private RabbitFanoutExchange? FanoutExchange { get; set; }
        private List<RabbitQueue>? FanoutQueues { get; set; }


        [TestMethod]
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
        public void PublishTo_RabbitFanoutExchange_Pass()
        {
            Create_CreateFanoutExchange_Pass();
            var producer = Client?.CreateProducer(FanoutExchange);
            producer?.PublishSingle(new RabbitMessage("duck.image.png", "quack!"));
            Assert.IsNotNull(producer);
        }
        [TestMethod]
        public void ConsumeFrom_RabbitDirectExchange_Pass()
        {
            Create_CreateFanoutExchange_Pass();

            var consumers = new List<RabbitConsumer>();
            FanoutQueues.ForEach(f => consumers.Add(Client.CreateConsumer(f)));

            var messages = new List<RabbitMessage>();

            consumers.ForEach(c => messages.Add(c.PullMessage(c.Queue)));

            Assert.IsNotNull(messages.Count == consumers.Count);
        }
    }
}
