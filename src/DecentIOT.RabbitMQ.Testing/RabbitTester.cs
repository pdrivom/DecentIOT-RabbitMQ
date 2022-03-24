using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecentIOT.RabbitMQ.Testing
{
    [TestClass]
    public class RabbitTester
    {
        internal RabbitClient? Client { get; set; }

        [TestMethod]
        internal void Create_RabbitVitualServerClient_Pass()
        {
            Client = new RabbitClient();
            Assert.IsTrue(Client.Connected);
        }

    }
}
