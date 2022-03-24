using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Message;
using DecentIOT.RabbitMQ.Queue;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace DecentIOT.RabbitMQ.Testing
{
    [TestClass]
    public class RabbitTopicExchangeTester
    {
        private RabbitClient? Client { get; set; }
        private RabbitTopicExchange? TopicExchange { get; set; }
        private RabbitQueue? TopicQueue { get; set; }


        [TestMethod]
        public void Create_RabbitVitualServerClient_Pass()
        {
            Client = new RabbitClient();
            Assert.IsTrue(Client.Connected);
        }
        [TestMethod]
        public void Create_CreateTopicExchange_Pass()
        {
            Create_RabbitVitualServerClient_Pass();
            TopicExchange = Client?.CreateTopicExchange("ex.topic");
            TopicQueue = TopicExchange?.CreateQueue("topic.canimal.queue", "*.image.*");
            Assert.IsNotNull(TopicQueue?.QueueDeclare);
        }

        [TestMethod]
        public void PublishTo_RabbitTopicExchange_Pass()
        {
            Create_CreateTopicExchange_Pass();
            var producer = Client.CreateProducer(TopicExchange);
            producer.PublishSingle(new RabbitMessage("duck.image.png", "quack!"));
            producer.PublishSingle(new RabbitMessage("dog.image.jpg", "auau!"));
            producer.PublishSingle(new RabbitMessage("cat.image.giff", "miau!"));
            Assert.IsNotNull(producer);
        }
        [TestMethod]
        public void ConsumeFrom_RabbitTopicExchange_Pass()
        {
            Create_CreateTopicExchange_Pass();

            var consumer = Client.CreateConsumer(TopicQueue);

            var duckMessage = consumer.PullMessage(TopicQueue);
            var dogMessage = consumer.PullMessage(TopicQueue);
            var catMessage = consumer.PullMessage(TopicQueue);

            Assert.IsNotNull(duckMessage);
            Assert.IsNotNull(dogMessage);
            Assert.IsNotNull(catMessage);
        }
    }
}