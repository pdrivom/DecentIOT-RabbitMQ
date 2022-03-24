using DecentIOT.RabbitMQ.Client.Models.Responser;
using DecentIOT.RabbitMQ.Client.Requester;
using DecentIOT.RabbitMQ.Message;
using DecentIOT.RabbitMQ.Requester;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecentIOT.RabbitMQ.Testing
{
    [TestClass]
    public class RabbitRequestResponseTester:RabbitTester
    {

        private RabbitResponser Responser { get; set; }
        private RabbitRequester Requester { get; set; }

        [TestMethod]
        public void Create_CreateResponser_Pass()
        {
            Create_RabbitVitualServerClient_Pass();
            Responser = Client?.CreateResponser("ex.reqresp", "animals");
            Assert.IsNotNull(Responser);
        }
        [TestMethod]
        public void Create_CreateRequester_Pass()
        {
            Create_RabbitVitualServerClient_Pass();
            Requester = Client?.CreateRequester("ex.reqresp", "animals");
            Assert.IsNotNull(Requester);
        }
        [TestMethod]
        public void Send_Request_Pass()
        {
            Create_CreateRequester_Pass();
            Requester.Request("dogs");
        }
        [TestMethod]
        public void GetRequestAndRespond_Pass()
        {
            Create_CreateResponser_Pass();
            var request = Responser.GetRequest();
            Assert.IsNotNull(request);
            Assert.IsTrue((request.Content).Contains("dogs"));
            var response = new RabbitMessage(request.ResponseKey, new List<string> { "Poodle", "German Shepard", "Border Colie" });
            Responser.Respond(response);
        }
        [TestMethod]
        public void Get_Response_Pass()
        {
            Create_CreateRequester_Pass();
            var response = Requester.GetResponse();
            Assert.IsNotNull(response);
        }
    }
}
