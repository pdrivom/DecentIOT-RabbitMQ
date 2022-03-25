using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Message;
using DecentIOT.RabbitMQ.Queue;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;

namespace DecentIOT.RabbitMQ.Testing
{
    [TestClass]
    public class RabbitTopicExchangeTester:RabbitTester
    {
        private RabbitTopicExchange? TopicExchange { get; set; }
        private RabbitQueue? TopicQueue { get; set; }

        public void Create_CreateTopicExchange_Pass()
        {
            Create_RabbitVitualServerClient_Pass();
            TopicExchange = Client?.CreateTopicExchange("ex.topic");
            TopicQueue = TopicExchange?.CreateQueue("topic.canimal.queue", "*.image.*");
            Assert.IsNotNull(TopicQueue?.QueueDeclare);
        }

        [TestMethod]
        public void T01_PublishTo_RabbitTopicExchange_Pass()
        {
            Create_CreateTopicExchange_Pass();
            var producer = Client.CreateProducer(TopicExchange);
            var messages = new List<RabbitMessage>
            {
                new RabbitMessage("duck.image.png", "quack!"),
                new RabbitMessage("dog.image.jpg", "auau!"),
                new RabbitMessage("cat.image.giff", "miau!")
            };
            producer.PublishMany(messages);
            Assert.IsNotNull(producer);
        }
        [TestMethod]
        public void T02_ConsumeFrom_RabbitTopicExchange_Pass()
        {
            Create_CreateTopicExchange_Pass();

            var consumer = Client.CreateConsumer(TopicQueue);

            var duckMessage = consumer.PullMessage();
            var dogMessage = consumer.PullMessage();
            var catMessage = consumer.PullMessage();

            Assert.IsNotNull(duckMessage);
            Assert.IsNotNull(dogMessage);
            Assert.IsNotNull(catMessage);
        }
    }
}