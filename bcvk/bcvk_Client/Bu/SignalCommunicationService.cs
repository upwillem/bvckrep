using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region ManuallyAdded
using Thrift.Protocol;
using Thrift.Transport;
using bcvkSignal;

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

        public SignalCommunicationService()
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
    }
}
