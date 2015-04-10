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
        public event Action<List<Bitmap>, List<byte[]>> participantBufferReady;

        private Webcam webcam;
        private Converter converter;
        private List<byte[]> videoBuffer;
        private List<byte[]> audioBuffer;

        private static Mutex streamMutex;

        #region Thrift classes
        //Thrift classes
        private TTransport transportStream;
        private TProtocol protocolStream;
        private Stream.Client streamClient;
        private int streamPort = 8080;
        #endregion

        public StreamCommunicationService()
        {
            streamMutex = new Mutex();

            webcam = new Webcam();
            webcam.frameReady += webcam_frameReady;

            converter = new Converter();

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

            SignalCommunicationService.connectionEstablished += SignalCommunicationService_connectionEstablished;
        }

        /// <summary>
        /// Luc Schnabel 1207776, Aron Huntjens 1209361
        /// manage the buffering
        /// </summary>
        /// <param name="state"></param>
        private void SignalCommunicationService_connectionEstablished(string state)
        {
            if (streamMutex.WaitOne())
            {
                Thread pollGetParticipantVideoBuffer = new Thread(() => GetParticipantVideoBuffer());
                if (state == "established")
                {
                    pollGetParticipantVideoBuffer.Start();
                }
                else if (state == "connectionended")
                {
                    pollGetParticipantVideoBuffer.Abort();
                }
                streamMutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Aron Huntjens 1209361
        /// </summary>
        private void GetParticipantVideoBuffer()
        {
            while (true)
            {
                if (streamMutex.WaitOne())
                {
                    if (AccountData.Instance.ConnectionEstablishedStatus == "established")
                    {
                        //get video (no audio)
                        AccountData acc = AccountData.Instance;
                        //dummycode
                        List<byte[]> video = streamClient.GetStream("2", "", acc.ConnectionId, false);

                        List<Bitmap> bitmaps = new List<Bitmap>();
                        foreach (byte[] fragment in video)
                        {
                            bitmaps.Add(converter.ToBitmap(fragment, 532, 399));
                        }
                        if (bitmaps.Count != 0)
                        {
                            participantBufferReady(bitmaps, null);
                        }
                    }
                    streamMutex.ReleaseMutex();
                }
                Thread.Sleep(4000);
            }


            //while (true)
            //{
            //    if (AccountData.Instance.ConnectionEstablishedStatus == "established")
            //    {
            //        List<byte[]> video = new List<byte[]>();
            //        video = streamClient.GetStream("3", AccountData.Instance.AccountId, AccountData.Instance.ConnectionId, false);
            //        if (stream.Count > 0)
            //        {
            //            //TODO: join this event (streamcontrolclass)
            //            participantBufferReady(video);
            //        }
            //    }
            //    Thread.Sleep(5);

            //}
        }

        /// <summary>
        /// New frame is ready (thread)
        /// </summary>
        /// <param name="bmp">bitmap of the frame</param>
        private void webcam_frameReady(Bitmap bmp)
        {
            frameReady(bmp);
            videoBuffer.Add(converter.ToByteArray((Image)bmp));
            if (videoBuffer.Count == 150)
            {
                sendVideoBuffer(videoBuffer);
                videoBuffer.Clear();
            }
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// sends the buffer to the server
        /// </summary>
        /// <param name="videoBuffer"></param>
        private void sendVideoBuffer(List<byte[]> videoBuffer)
        {
            if (streamMutex.WaitOne())
            {
                if (AccountData.Instance.ConnectionEstablishedStatus == "established")
                {
                    streamClient.SendStream(AccountData.Instance.AccountId, ""/*OPTIONAL*/,
                        videoBuffer, AccountData.Instance.ConnectionId, false);
                }
                streamMutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Start the webcam and microphone
        /// </summary>
        public void StartCapture()
        {
            webcam.StartCamera();
            //TODO: start microphone
        }

        /// <summary>
        /// Stop the webcam and microphone
        /// </summary>
        public void StopCapture()
        {
            webcam.StopCamera();
            //TODO: stop micophone
        }

        /// <summary>
        /// Stop the webcam and free it when application closes
        /// </summary>
        public void On_Application_Ended()
        {
            webcam.On_Application_End();
            //TODO: stop microphone
        } 

        public void SetVideoMessage(string recipient) 
        {
            throw new NotImplementedException();
        }
        public void GetVideoMessage(string videoMessageId)
        {
            throw new NotImplementedException();
        }
        public void GetStreamAsParent(string recipient, string connectionId)
        {
            throw new NotImplementedException();
        }
    }
}
