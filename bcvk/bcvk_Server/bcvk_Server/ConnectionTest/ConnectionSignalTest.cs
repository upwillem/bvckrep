using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cc;

namespace ConnectionTest
{
    [TestClass]
    public class ConnectionSignalTest
    {
        [TestMethod]
        public void TestReceiveIdDoCall()
        {
            int callid = CommunicationHandler.DoCall("1", "2");
            bool biggerThen0 =false;
            if (callid>0){
                biggerThen0=true;
            }
            Assert.AreEqual(biggerThen0, true);            
        }

        [TestMethod]
        public void TestGetConnectionState()
        {
            int callid = CommunicationHandler.DoCall("1", "2");
            string status = CommunicationHandler.GetConnetionState(callid);
            Assert.AreEqual(status,"establishing");
        }

        [TestMethod]
        public void TestAnwserCall1()
        {
            int callid = CommunicationHandler.DoCall("1", "2");
            CommunicationHandler.AnwserCall("2", callid, "connected");
            string status = CommunicationHandler.GetConnetionState(callid);
            Assert.AreEqual(status, "established");
        }

        [TestMethod]
        public void TestAnwserCall2()
        {
            int callid = CommunicationHandler.DoCall("1", "2");
            CommunicationHandler.AnwserCall("2", callid, "connected");
            string status = CommunicationHandler.GetConnetionState(callid,"2");
            Assert.AreEqual(status, "connected");
        }





    }
}
