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
        /// <param name="username"></param>
        public void PollAccountData(string username)
        {
            while (true)
            {
                List<string> listAccountData = new List<string>();
                listAccountData = signalClient.GetAccountData(username);
                if ((listAccountData != null) && (listAccountData.Count != 0))
                { 
                    accountDataListReady(listAccountData); 
                }
                Thread.Sleep(1);
            }
        }

        public string PollConnection(string connectionId, string username)
        {
            throw new NotImplementedException();
        }

        public string PollConnection(string connectionId)
        {
            throw new NotImplementedException();
        }
    }
}
