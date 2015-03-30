using Bu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cc
{
    public class SignalControlClass
    {
        private SignalCommunicationService signalCommunicationService;
        private string USERNAME;

        /// <summary>
        /// Constructor
        /// </summary>
        public SignalControlClass(string USERNAME) 
        {
            this.USERNAME = USERNAME;
            signalCommunicationService = new SignalCommunicationService();
            //GetAccountData();
        }

        /// <summary>
        /// Get the most current accountdata
        /// </summary>
        private void GetAccountData()
        {
            signalCommunicationService.GetAccountData(USERNAME);
        }

        public void StartPolling() 
        {
            throw new NotImplementedException();
        }

        public string Answer(string p1, string p2, string p3, string p4)
        { 
            throw new NotImplementedException();
        }

        public string GetCallStatus()
        {
            throw new NotImplementedException();
        }

        public string DoCall(string p1, string p2)
        {
            throw new NotImplementedException();
        }
    }
}
