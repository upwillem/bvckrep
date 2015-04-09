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
        public event Action<List<byte[]>> participantBufferReady;
        private Thread sendStream;

        /// <summary>
        /// Constructor
        /// </summary>
        public StreamControlClass() 
        {
            streamCommunicationService = new StreamCommunicationService();
            streamCommunicationService.frameReady += streamCommunicationService_frameReady;
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
            sendStream.Abort();
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
