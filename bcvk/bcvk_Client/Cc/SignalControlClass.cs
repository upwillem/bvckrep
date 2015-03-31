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
        public SignalControlClass(string username)
        {
            AccountData acc = AccountData.Instance;
            acc.Username = username;
            signalCommunicationService = new SignalCommunicationService(username);
            signalCommunicationService.accountDataListReady += signalCommunicationService_accountDataListReady;
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// creates the thread of polling the accountdata
        /// </summary>
        /// <param name="username">username of the current user</param>
        public void StartPoll()
        {
            Thread pollThread = new Thread(() => signalCommunicationService.PollAccountData(AccountData.Instance.Username));
            pollThread.Start();
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// </summary>
        /// <param name="accountDataList"></param>
        private void signalCommunicationService_accountDataListReady(List<string> accountDataList)
        {
            throw new NotImplementedException();
        }
    }
}
