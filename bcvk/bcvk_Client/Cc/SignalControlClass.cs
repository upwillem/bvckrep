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
        public event Action<string> initContacts;
        public event Action<string> beingCalled;
        private Thread pollThread;

        public Thread PollThread
        { get { return pollThread; } }

        public string Username
        { get { return AccountData.Instance.Username; } }


        public string ConnectionId 
        { get { return AccountData.Instance.Connection; } }

        //TODO: Throw this away
        public string AccountId
        { get { return AccountData.Instance.AccountId; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public SignalControlClass(string username)
        {
            //create singleton object
            AccountData acc = AccountData.Instance;
            acc.Username = username;

            signalCommunicationService = new SignalCommunicationService(username);
            //Get the account data
            signalCommunicationService.accountDataListReady += signalCommunicationService_accountDataListReady;
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

        /// <summary>
        /// Luc Schnabel 1207776,
        /// answer the received call
        /// </summary>
        /// <param name="sender">the sender of the answer</param>
        /// <param name="contact">contact to send the answer to</param>
        /// <param name="connectionId">the connectionId of the call</param>
        /// <param name="answer">the answer of the sender</param>
        public void AnswerCall(string sender, string contact, string connectionId, string answer)
        {
            signalCommunicationService.AnswerCall(sender, contact, connectionId, answer);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// creates the thread of polling the accountdata
        /// </summary>
        /// <param name="username">username of the current user</param>
        public void StartPoll()
        {
            pollThread = new Thread(() => signalCommunicationService.PollAccountData(AccountData.Instance.Username));
            pollThread.Start();
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// sets all values of 'AccountData'.
        /// </summary>
        /// <param name="accountDataList">list of all accountdata</param>
        private void signalCommunicationService_accountDataListReady(List<string> accountDataList)
        {
            AccountData acc = AccountData.Instance;
            string oldContacts = acc.Contacts;
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

            if (oldContacts != acc.Contacts)
                initContacts(acc.Contacts);

            if (AccountData.Instance.Connection == "connecting") 
            {
                beingCalled("Je wordt gebeld");
            }
        }
    }
}
