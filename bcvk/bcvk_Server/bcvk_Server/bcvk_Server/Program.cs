using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bcvkSignal;
using bcvkStream;
using Thrift;
using Thrift.Transport;
using Thrift.Server;
using System.Threading;

namespace bcvk_Server
{

    public class Program
    {
        static ApplicationManager applicationManager;


        /// <summary>
        /// Aron Huntjens 1209361
        /// Main kick to server  
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            try
            {
                applicationManager = new ApplicationManager();

                Thread signalThread = new Thread(() => DoWorkSignal(applicationManager));
                signalThread.Start();

                Thread streamThread = new Thread(() => DoWorkStream(applicationManager));
                streamThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Roel Larik 1236830
        /// Signal thread 
        /// </summary>
        /// <param name="applicationManager">main application manager</param>
        private static void DoWorkSignal(ApplicationManager applicationManager)
        {
            Signal.Processor signalProcessor = new Signal.Processor(applicationManager);
            var signalServerPort = 9090;
            TServerTransport signalServerTransport = new TServerSocket(signalServerPort);
            TServer signalServer = new TSimpleServer(signalProcessor, signalServerTransport);
            Console.WriteLine("signalserver listening on port: " + signalServerPort);
            signalServer.Serve();
        }

        /// <summary>
        /// Roel Larik 1236830
        /// Stream thread 
        /// </summary>
        /// <param name="applicationManager">main application manager</param>
        private static void DoWorkStream(ApplicationManager applicationManager)
        {
            Stream.Processor streamProcessor = new Stream.Processor(applicationManager);
            var streamServerPort = 8080;
            TServerTransport streamServerTransport = new TServerSocket(streamServerPort);
            TServer streamSever = new TSimpleServer(streamProcessor, streamServerTransport);
            Console.WriteLine("streamserver listening on port: " + streamServerPort);
            streamSever.Serve();
        }
    }
}
