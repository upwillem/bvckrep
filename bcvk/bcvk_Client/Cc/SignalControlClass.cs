using Bu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cc
{
    public class SignalControlClass
    {
        private SignalCommunicationService signalCommunicationService;

        /// <summary>
        /// Constructor
        /// </summary>
        public SignalControlClass() 
        {
            signalCommunicationService = new SignalCommunicationService();
            StartPoll("l");
            signalCommunicationService.accountDataListReady += signalCommunicationService_accountDataListReady;
        }

        private void signalCommunicationService_accountDataListReady(List<string> obj)
        {
            throw new NotImplementedException();
        }

        public void StartPoll(string username)
        {
            Thread pollThread = new Thread(() => signalCommunicationService.PollAccountData(username));
            pollThread.Start();
        }

        
    }
}
