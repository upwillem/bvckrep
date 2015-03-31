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
            signalCommunicationService.accountDataListReady += signalCommunicationService_accountDataListReady;
            signalCommunicationService.connectionParticipantStateReady += signalCommunicationService_connectionParticipantStateReady;
            signalCommunicationService.connectionStateReady +=signalCommunicationService_connectionStateReady;
            
        }

        private void signalCommunicationService_connectionStateReady(string obj)
        {
            throw new NotImplementedException();
        }

        private void signalCommunicationService_connectionParticipantStateReady(string obj)
        {
            throw new NotImplementedException();
        }


        private void signalCommunicationService_accountDataListReady(List<string> accountDataList)
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
