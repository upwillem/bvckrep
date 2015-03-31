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
            //create singleton object
            AccountData acc = AccountData.Instance;
            acc.Username = username;

            signalCommunicationService = new SignalCommunicationService(username);
            signalCommunicationService.accountDataListReady += signalCommunicationService_accountDataListReady;
            signalCommunicationService.connectionParticipantStateReady += signalCommunicationService_connectionParticipantStateReady;       
            signalCommunicationService.connectionStateReady +=signalCommunicationService_connectionStateReady;
            
        }

        /// <summary>
        /// Aron Huntjens 1209361
        /// Call a client
        /// </summary>
        /// <param name="contact">client to call</param>
        public void DoCall(string contact)
        {
            signalCommunicationService.Docall(contact);
        }





        private void signalCommunicationService_connectionParticipantStateReady(string obj)
        {
            throw new NotImplementedException();
        }

        private void signalCommunicationService_connectionStateReady(string obj)
        {
            throw new NotImplementedException();
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
            AccountData acc = AccountData.Instance;
            foreach (string data in accountDataList)
            {
                #region switch cases accountdatalist
                switch (data.Split(';')[0])
                {
                    case "accountId":
                        acc.AccountId = data.Split(';')[1];
                        break;
                    case "parentId":
                        acc.ParentId = data.Split(';')[1];
                        break;
                    case "username":
                        acc.Username = data.Split(';')[1];
                        break;
                    case "email":
                        acc.Email = data.Split(';')[1];
                        break;
                    case "displayName":
                        acc.DisplayName = data.Split(';')[1];
                        break;
                    case "phonenumber":
                        acc.Phonenumber = data.Split(';')[1];
                        break;
                    case "photo":
                        acc.Photo = data.Split(';')[1];
                        break;
                    case "connection":
                        acc.Connection = data.Split(';')[1];
                        break;
                    case "log":
                        acc.Log = data.Split(';')[1];
                        break;
                    case "children":
                        acc.Children = data.Split(';')[1];
                        break;
                    case "contacts":
                        acc.Contacts = data.Split(';')[1];
                        break;
                    default:
                        break;
                }
                #endregion
            }

            if (AccountData.Instance.Connection == "connecting") 
            { 
                
            }
            else if (AccountData.Instance.Connection == "connected") 
            { 
                
            }
        }
    }
}
