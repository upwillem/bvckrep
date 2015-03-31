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

        public event Action<List<string>> accountDataListReady;
        public event Action<string> connectionStateReady;
        public event Action<string> connectionParticipantStateReady;

        public SignalCommunicationService(string username)
        {
            #region Declaration Thrift classes and open transport
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
            #endregion
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// polls the accountdata. If data is received, a event is triggered.
        /// </summary>
        /// <param name="username">username</param>
        public void PollAccountData(string username)
        {
            //Thread
            while (true)
            {
                List<string> listAccountData = new List<string>();
                listAccountData = signalClient.GetAccountData(username);
                
                if ((listAccountData != null) && (listAccountData.Count != 0))
                { 
                    accountDataListReady(listAccountData); 
                }
                Thread.Sleep(5);
            }
        }

        /// <summary>
        /// Aron Huntjens 1209361 this method pols if a user is in an specific connection
        /// </summary>
        /// <param name="connectionId">connection identification token</param>
        /// <param name="username">specific user</param>
        public void PollUserConnectionState(string connectionId)
        {
            while (true)
            {
                if (!string.IsNullOrEmpty(connectionId))
                {
                    string state = signalClient.GetParticipantCallStatus(connectionId, AccountData.Instance.Username);
                    connectionParticipantStateReady(state);
                }
                Thread.Sleep(5);
            }                   
        }

        /// <summary>
        /// Aron Huntjens 1209361 this method polls the state of a connection.
        /// Polling stops automatically when a connection is ended.
        /// </summary>
        /// <param name="connectionId">connection identification token</param>
        public void PollConnection(string connectionId)
        {
            bool keepPolling = true;
            AccountData acc = AccountData.Instance;
            while (keepPolling)
            {
                if (!string.IsNullOrEmpty(connectionId))
                {
                    string state = signalClient.GetCallStatus(connectionId);
                    acc.ConnectionStatus = state;
                    if (state == "connectionended")
                    {
                        keepPolling = false;
                    }
                    connectionStateReady(state);
                }
                Thread.Sleep(5);
            }
        }

        /// <summary>
        /// Aron Huntjens 1209361 
        /// Call a client
        /// </summary>
        /// <param name="contact">client to call</param>
        public void Docall(string contact)
        {
            AccountData acc = AccountData.Instance;
            string connectionId= signalClient.DoCall(acc.AccountId, contact);
            acc.Connection = connectionId;
            PollConnection(connectionId);
        }
    }
}
