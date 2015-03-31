using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cc;
using Bu;

namespace ConnectionTest
{
    [TestClass]
    public class ConnectionSignalTest
    {
        [TestMethod]
        public void TestReceiveIdDoCall()
        {
            string connectionId = CommunicationHandler.DoConnect("1", "2");            
            Assert.AreEqual(String.IsNullOrEmpty(connectionId), false);
            CommunicationHandler.EndConnection(connectionId);
        }

        [TestMethod]
        public void TestGetConnectionState()
        {
            string connectionId = CommunicationHandler.DoConnect("1", "2");
            string status = CommunicationHandler.GetConnectionState(connectionId);
            Assert.AreEqual(status,"establishing");
            CommunicationHandler.EndConnection(connectionId);
        }

        [TestMethod]
        public void TestAnwserConnection1()
        {
            string connectionId = CommunicationHandler.DoConnect("1", "2");
            CommunicationHandler.AnwserConnection("2", connectionId, "connected");
            string status = CommunicationHandler.GetConnectionState(connectionId);
            Assert.AreEqual(status, "established");
            CommunicationHandler.EndConnection(connectionId);
        }

        [TestMethod]
        public void TestAnwserConntection2()
        {
            string connectionId = CommunicationHandler.DoConnect("1", "2");
            CommunicationHandler.AnwserConnection("2", connectionId, "discconected");
            string status = CommunicationHandler.GetConnectionState(connectionId);
            Assert.AreEqual(status, "connectionended");
            CommunicationHandler.EndConnection(connectionId);
        }

        [TestMethod]
        public void TestAnwserConnection3()
        {
            string connectionId = CommunicationHandler.DoConnect("1", "2");
            CommunicationHandler.AnwserConnection("2", connectionId, "connected");
            string status = CommunicationHandler.GetConnectionState(connectionId,"2");
            Assert.AreEqual(status, "connected");
            CommunicationHandler.EndConnection(connectionId);
        }

        [TestMethod]
        public void TestAnwserConnection4()
        {
            string connectionId = CommunicationHandler.DoConnect("1", "2");
            CommunicationHandler.AnwserConnection("2", connectionId, "disconnected");
            string status = CommunicationHandler.GetConnectionState(connectionId, "2");
            Assert.AreEqual(status, "disconnected");
            CommunicationHandler.EndConnection(connectionId);
        }



        [TestMethod]
        public void EndConnection()
        {
            string connectionId = CommunicationHandler.DoConnect("1", "2");
            CommunicationHandler.EndConnection(connectionId);
            Connection connection = CommunicationHandler.Connections.Find(x => x.Id == connectionId);
            Assert.AreEqual(connection, null);
            
        }





    }
}
