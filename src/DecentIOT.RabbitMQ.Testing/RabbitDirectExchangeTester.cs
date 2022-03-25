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
    public class RabbitDirectExchangeTester: RabbitTester
    {
        private RabbitDirectExchange? DirectExchange { get; set; }
        private RabbitQueue? DirectQueue { get; set; }



        public void Create_CreateDirectExchange_Pass()
        {
            Create_RabbitVitualServerClient_Pass();
            DirectExchange = Client?.CreateDirectExchange("ex.direct");
            DirectQueue = DirectExchange?.CreateQueue("direct.animal.queue", "duck.image.png");
            Assert.IsNotNull(DirectQueue?.QueueDeclare);
        }
        [TestMethod]
        public void T01_PublishTo_RabbitDirectExchange_Pass()
        {
            Create_CreateDirectExchange_Pass();
            var producer = Client?.CreateProducer(DirectExchange);
            var messages = new List<RabbitMessage>
            {
                new RabbitMessage("duck.image.png", "quack!"),
                new RabbitMessage("duck.image.png", "shit!"),
                new RabbitMessage("duck.image.png", "a wolf!")
            };
            producer?.PublishMany(messages);
            Assert.IsNotNull(producer);
        }
        [TestMethod]
        public void T02_ConsumeFrom_RabbitDirectExchange_Pass()
        {
            Create_CreateDirectExchange_Pass();

            var consumer = Client.CreateConsumer(DirectQueue);

            var duck1Message = consumer.PullMessage();
            var duck2Message = consumer.PullMessage();
            var duck3Message = consumer.PullMessage();

            Assert.IsNotNull(duck1Message);
            Assert.IsNotNull(duck2Message);
            Assert.IsNotNull(duck3Message);
        }
    }
}
