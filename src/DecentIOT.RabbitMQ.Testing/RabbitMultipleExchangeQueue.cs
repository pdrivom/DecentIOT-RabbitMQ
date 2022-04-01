using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecentIOT.RabbitMQ.Testing
{
    [TestClass]
    public  class RabbitMultipleExchangeQueue:RabbitTester
    {
        [TestMethod]
        public void AddSameQueueOnMultipleExchanges_Pass()
        {
            Create_RabbitVitualServerClient_Pass();
            var topicExchange = Client?.CreateTopicExchange("ex.topic");
            var queue = topicExchange?.CreateQueue("topic.animal.queue", "*.image.*");
            Assert.IsNotNull(queue?.QueueDeclare);
            var directExchange = Client?.CreateDirectExchange("ex.direct");
            directExchange.AddQueue(queue, "duck.image.png");
        }
    }
}
