using Bu;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cc
{
    public class StreamControlClass
    {
        private StreamCommunicationService streamCommunicationService;
        public event Action<Bitmap> frameReady;
        public event Action<List<Bitmap>,List<byte[]>> participantBufferReady;
        private Thread sendStream; //why is this ?

        /// <summary>
        /// Constructor
        /// </summary>
        public StreamControlClass() 
        {
            streamCommunicationService = new StreamCommunicationService();
            streamCommunicationService.frameReady += streamCommunicationService_frameReady;
            streamCommunicationService.participantBufferReady += streamCommunicationService_participantBufferReady;
         
        }

        //rethrow event
        private void streamCommunicationService_participantBufferReady(List<Bitmap> bitmaps, List<byte[]> audio)
        {
            participantBufferReady(bitmaps, audio);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        private void streamCommunicationService_frameReady(Bitmap bmp)
        {
            frameReady(bmp);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartCapture()
        {
            streamCommunicationService.StartCapture();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopCamera()
        {
            //sendStream.Abort(); //why is this ?
            streamCommunicationService.StopCapture();
        }

        /// <summary>
        /// 
        /// </summary>
        public void On_Application_Ended()
        {
            streamCommunicationService.On_Application_Ended();
        }
    }
}
