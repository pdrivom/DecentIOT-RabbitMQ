using DecentIOT.RabbitMQ.Responder;
using DecentIOT.RabbitMQ.Requester;
using DecentIOT.RabbitMQ.Message;
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

        private RabbitResponder Responder { get; set; }
        private RabbitRequester Requester { get; set; }

        public void Create_CreateResponser_Pass()
        {
            Create_RabbitVitualServerClient_Pass();
            Responder = Client?.CreateResponser("ex.reqresp", "animals");
            Assert.IsNotNull(Responder);
        }
        public void Create_CreateRequester_Pass()
        {
            Create_RabbitVitualServerClient_Pass();
            Requester = Client?.CreateRequester("ex.reqresp", "animals");
            Assert.IsNotNull(Requester);
        }
        [TestMethod]
        public void T01_Send_Request_Pass()
        {
            Create_CreateRequester_Pass();
            Requester.SendRequest("dogs");
        }
        [TestMethod]
        public void T02_GetRequestAndRespond_Pass()
        {
            Create_CreateResponser_Pass();
            var request = Responder.GetRequest();
            Assert.IsNotNull(request);
            Assert.IsTrue((request.Content).Contains("dogs"));
            Responder.SendResponse(request, new List<string> { "Poodle", "German Shepard", "Border Colie" });
        }
        [TestMethod]
        public void T03_Get_Response_Pass()
        {
            Create_CreateRequester_Pass();
            var response = Requester.GetResponse();

            Assert.IsNotNull(response.Content);
        }
    }
}
