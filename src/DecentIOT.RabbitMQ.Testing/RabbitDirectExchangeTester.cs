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
    public class RabbitDirectExchangeTester
    {
        private RabbitClient? Client { get; set; }
        private RabbitDirectExchange? DirectExchange { get; set; }
        private RabbitQueue? DirectQueue { get; set; }

        [TestMethod]
        public void Create_RabbitVitualServerClient_Pass()
        {
            Client = new RabbitClient();
            Assert.IsTrue(Client.Connected);
        }

        [TestMethod]
        public void Create_CreateDirectExchange_Pass()
        {
            Create_RabbitVitualServerClient_Pass();
            DirectExchange = Client?.CreateDirectExchange("ex.direct");
            DirectQueue = DirectExchange?.CreateQueue("direct.animal.queue", "duck.image.png");
            Assert.IsNotNull(DirectQueue?.QueueDeclare);
        }
        [TestMethod]
        public void PublishTo_RabbitDirectExchange_Pass()
        {
            Create_CreateDirectExchange_Pass();
            var producer = Client?.CreateProducer(DirectExchange);
            producer?.PublishSingle(new RabbitMessage("duck.image.png", "quack!"));
            producer?.PublishSingle(new RabbitMessage("duck.image.png", "shit!"));
            producer?.PublishSingle(new RabbitMessage("duck.image.png", "a wolf!"));
            Assert.IsNotNull(producer);
        }
        [TestMethod]
        public void ConsumeFrom_RabbitDirectExchange_Pass()
        {
            Create_CreateDirectExchange_Pass();

            var consumer = Client.CreateConsumer(DirectQueue);

            var duck1Message = consumer.PullMessage(DirectQueue);
            var duck2Message = consumer.PullMessage(DirectQueue);
            var duck3Message = consumer.PullMessage(DirectQueue);

            Assert.IsNotNull(duck1Message);
            Assert.IsNotNull(duck2Message);
            Assert.IsNotNull(duck3Message);
        }
    }
}
