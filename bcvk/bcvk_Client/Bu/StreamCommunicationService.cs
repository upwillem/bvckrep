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
#endregion

namespace Bu
{
    public class StreamCommunicationService
    {
        public event Action<Bitmap> frameReady;

        private Webcam webcam;

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

                streamClient = new bcvkStream.Stream.Client(protocolStream);

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
        }

        /// <summary>
        /// New frame is ready (bitmap format)
        /// </summary>
        /// <param name="bAFrame"></param>
        private void webcam_byteArrayReady(byte[] bA)
        {
            throw new NotImplementedException();
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
