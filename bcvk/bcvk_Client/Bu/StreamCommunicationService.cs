using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region ManuallyAdded
using Thrift.Protocol;
using Thrift.Transport;
using bcvkStream;
using System.Drawing;
using System.Threading;
#endregion

namespace Bu
{
    public class StreamCommunicationService
    {
        public event Action<Bitmap> frameReady;

        private Webcam webcam;
        private List<byte[]> videoBuffer;
        private List<byte[]> audioBuffer;

        #region Thrift classes
        //Thrift classes
        private TTransport transportStream;
        private TProtocol protocolStream;
        private Stream.Client streamClient;
        private int streamPort = 8080;
        #endregion

        public StreamCommunicationService()
        {
            webcam = new Webcam();
            webcam.frameReady += webcam_frameReady;
            webcam.byteArrayReady += webcam_byteArrayReady;

            #region Declaration Thrift classes and open transport
            try
            {
                //Stream settings
                transportStream = new TSocket("localhost", streamPort);
                protocolStream = new TBinaryProtocol(transportStream);

                streamClient = new Stream.Client(protocolStream);

                transportStream.Open();
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

            videoBuffer = new List<byte[]>();
            audioBuffer = new List<byte[]>();
        }

        /// <summary>
        /// New frame is ready (bitmap format)
        /// </summary>
        /// <param name="bAFrame"></param>
        private void webcam_byteArrayReady(byte[] bA)
        {
            videoBuffer.Add(bA);
            if (videoBuffer.Count == 15)
            {
                sendBuffer(videoBuffer);
                videoBuffer.Clear();
            }
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// sends the buffer to the server
        /// </summary>
        /// <param name="videoBuffer"></param>
        private void sendBuffer(List<byte[]> videoBuffer)
        {
            if (AccountData.Instance.ConnectionEstablishedStatus == "established")
            {
                streamClient.SendStream(AccountData.Instance.Username, "3",
                    videoBuffer, AccountData.Instance.Connection, false);
            }
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// gets the buffer from the server
        /// </summary>
        /// <param name="videoBuffer"></param>
        public void GetBuffer()
        {
            while (true)
            {
                if (AccountData.Instance.ConnectionEstablishedStatus == "established")
                {
                    List<byte[]>stream = new List<byte[]>();
                    stream   = streamClient.GetStream("3", AccountData.Instance.AccountId, AccountData.Instance.Connection, false);
                    if(stream.Count >0)
                    { 
                    
                    }
                }
                Thread.Sleep(1000);

            }
        }

        /// <summary>
        /// New frame is ready (byte[] format)
        /// </summary>
        /// <param name="bmp">bitmap of the frame</param>
        private void webcam_frameReady(Bitmap bmp)
        {
            frameReady(bmp);
        }

        #region Webcam functions
        /// <summary>
        /// Start the webcam
        /// </summary>
        public void StartCamera()
        {
            webcam.StartCamera();
        }

        /// <summary>
        /// Stop the webcam
        /// </summary>
        public void StopCamera()
        {
            webcam.StopCamera();
        }

        /// <summary>
        /// Stop the webcam and free it when application closes
        /// </summary>
        public void On_Application_Ended()
        {
            webcam.On_Application_End();
        } 
        #endregion
    }
}
