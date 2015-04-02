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
        public event Action<Bitmap> frameReady;
        private StreamCommunicationService streamCommunicationService;
        public event Action<List<byte[]>> bufferReceived;
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
        /// <param name="obj"></param>
        private void streamCommunicationService_frameReady(Bitmap bmp)
        {
            frameReady(bmp);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartCamera()
        {
            streamCommunicationService.bufferReceived += streamCommunicationService_bufferReceived;
            streamCommunicationService.StartCamera();
            Thread pollGetBuffer = new Thread(() => streamCommunicationService.GetBuffer());
            pollGetBuffer.Start();
        }

        private void streamCommunicationService_bufferReceived(List<byte[]> obj)
        {
            bufferReceived(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopCamera()
        {
            streamCommunicationService.StopCamera();
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
