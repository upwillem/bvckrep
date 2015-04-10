using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region ManuallyAdded
using Thrift.Protocol;
using Thrift.Transport;
using bcvkSignal;
using System.Threading;

#endregion

namespace Bu
{
    public class SignalCommunicationService
    {
        #region Thrift classes
        //Thrift classes
        private TTransport transportSignal;
        private TProtocol protocolSignal;
        private Signal.Client signalClient;
        private int signalPort = 9090;
        #endregion

        public event Action<List<string>> accountDataReady;
        public static event Action<string> connectionEstablished;
        public event Action<string> participantConnectionStateReady;

        private Thread pollAccountDataThread;
        private Thread pollConnectionThread;
        private Thread pollUserConnectionStateThread;

        public SignalCommunicationService()
        {
            try
            {
                //Signal settings
                transportSignal = new TSocket("localhost", signalPort);
                protocolSignal = new TBinaryProtocol(transportSignal);

                signalClient = new Signal.Client(protocolSignal);

                transportSignal.Open();
            }
            catch (System.Net.Sockets.SocketException exc)
            {
                //MessageBox.Show(exc.Message);
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// answer the call
        /// </summary>
        /// <param name="sender">the sender of the answer</param>
        /// <param name="recipient">recipient of the answer</param>
        /// <param name="connectionId">id of the connection</param>
        /// <param name="answer">answer</param>
        public void AnswerCall(string sender, string recipient, string connectionId, string answer)
        {
            AccountData acc = AccountData.Instance;
            signalClient.AnswerCall(acc.AccountId, ""/*optional*/, acc.ConnectionId, answer);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// Call a contact
        /// </summary>
        /// <param name="contact">contact to call</param>
        public void DoCall(string contact)
        {
            AccountData acc = AccountData.Instance;
            string connectionId = signalClient.DoCall(acc.AccountId, contact);
            acc.ConnectionId = connectionId;            
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// end the call
        /// </summary>
        public void EndCall()
        {
            signalClient.EndCall(AccountData.Instance.Username, ""/*not necessary*/,AccountData.Instance.ConnectionId);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// start all the pollthreads
        /// </summary>
        /// <param name="username"></param>
        public void StartPoll(string username)
        {
            AccountData account = AccountData.Instance;

            pollAccountDataThread = new Thread(() => PollAccountData(account.Username));
            pollAccountDataThread.Start();

            pollConnectionThread = new Thread(() => PollConnection());
            pollConnectionThread.Start();

            pollUserConnectionStateThread = new Thread(() => PollUserConnectionState());
            pollUserConnectionStateThread.Start();
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// add a contact
        /// </summary>
        /// <param name="recipient">contact</param>
        /// <returns>succes or failure</returns>
        public bool AddContact(string recipient)
        {
            return signalClient.AddContact(AccountData.Instance.Username, recipient);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// accept a contact request
        /// </summary>
        /// <param name="recipient">contact</param>
        /// <returns>succes or failure</returns>
        public bool AcceptContact(string recipient)
        {
            return signalClient.AcceptContact(AccountData.Instance.Username, recipient);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// delete a contact
        /// </summary>
        /// <param name="recipient">contact</param>
        /// <returns>succes or failure</returns>
        public bool DeleteContact(string recipient)
        {
            return signalClient.DeleteContact(AccountData.Instance.Username, recipient);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// Block or deblock a contact
        /// </summary>
        /// <param name="recipient"></param>
        /// <returns></returns>
        public bool ToggleBlock(string recipient)
        {
            return signalClient.ToggleBlock(AccountData.Instance.Username, recipient);
        }

        /// <summary>
        /// Luc Schnabel 1207776
        /// create a main account. This accoutn can edit the subaccount
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password1">password</param>
        /// <param name="password2">password repeat</param>
        /// <param name="email">email address</param>
        /// <param name="name">displayname</param>
        /// <param name="phoneNumber">phone number</param>
        /// <returns></returns>
        public List<string> CreateMainAccount(string username, string password1, string password2, string email, string name, string phoneNumber)
        {
            return signalClient.CreateMainAccount(username, password1, password2, email, name, phoneNumber);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// create a sub account. This account can chat with other sub accounts
        /// </summary>
        /// <param name="parentId">id of the parent</param>
        /// <param name="username">username</param>
        /// <param name="password1">password</param>
        /// <param name="password2">password repeat</param>
        /// <param name="name">displayname</param>
        /// <param name="profileImage">image for the profile</param>
        /// <returns></returns>
        public List<string> CreateSubAccount(int parentId, string username, string password1, string password2, string name, byte[] profileImage)
        {
            return signalClient.CreateSubAccount(parentId, username, password1, password2, name, profileImage);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// login to server
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public List<string> Login(string username, string password)
        {
            AccountData.Instance.Username = username;
            return signalClient.Login(username, password);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// logout from server
        /// </summary>
        /// <param name="username"></param>
        public void LogOut(string username)
        {
            signalClient.LogOut(username);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// aborts the threads
        /// </summary>
        /// <returns></returns>
        public bool AbortsThreads() 
        {
            if(pollAccountDataThread.IsAlive)
                pollAccountDataThread.Abort();
            if(pollConnectionThread.IsAlive)
                pollConnectionThread.Abort();
            if(pollUserConnectionStateThread.IsAlive)
                pollUserConnectionStateThread.Abort();

            if (pollAccountDataThread.IsAlive || pollConnectionThread.IsAlive || pollUserConnectionStateThread.IsAlive)
                return false;
            return true;
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// polls the accountdata. If data is received, a event is triggered.
        /// </summary>
        /// <param name="username">username</param>
        private void PollAccountData(string username)
        {
            while (true)
            {
                List<string> listAccountData = listAccountData = signalClient.GetAccountData(username);
                if ((listAccountData != null) && (listAccountData.Count != 0))
                {
                    accountDataReady(listAccountData);
                }
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// Aron Huntjens 1209361 this method polls if a user is in an specific connection
        /// </summary>
        /// <param name="connectionId">connection identification token</param>
        /// <param name="username">specific user</param>
        private void PollUserConnectionState()
        {
            while (true)
            {
                AccountData acc = AccountData.Instance;
                if (!string.IsNullOrEmpty(acc.ConnectionId))
                {
                    string state = signalClient.GetParticipantCallStatus(acc.ConnectionId, acc.AccountId);
                    participantConnectionStateReady(state);
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Aron Huntjens 1209361 this method polls the state of a connection.
        /// Polling stops automatically when a connection is ended.
        /// </summary>
        /// <param name="connectionId">connection identification token</param>
        private void PollConnection()
        {
            string oldState = "";
            bool keepPolling = true;
            AccountData acc = AccountData.Instance;
            while (keepPolling)
            {
                string connectionId = acc.ConnectionId;
                if (connectionId != null && connectionId.Trim().Length != 0)
                {
                    string state = signalClient.GetCallStatus(connectionId);
                    acc.ConnectionEstablishedStatus = state;
                    if (state != null && state == "established" && state != oldState)
                    {
                        AccountData.Instance.ConnectionEstablishedStatus = "established";
                        connectionEstablished("established");
                    }
                    else if ((state == "connectionended") && (state != oldState))
                    {
                        keepPolling = false;
                        connectionEstablished("connectionended");
                        //TODO: REVISE RESETABORT()
                        Thread.ResetAbort();
                    }
                    oldState = state;
                }
                Thread.Sleep(100000);
            }
        }
    }
}
