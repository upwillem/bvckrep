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
    /// <summary>
    /// Owner: Luc Schnabel 1207776
    /// </summary>
    public class StreamControlClass
    {
        private StreamCommunicationService streamCommunicationService;
        public event Action<Bitmap> frameReady;
        public event Action<List<Bitmap>,List<byte[]>> participantBufferReady;

        /// <summary>
        /// Constructor
        /// </summary>
        public StreamControlClass() 
        {
            streamCommunicationService = new StreamCommunicationService();
            streamCommunicationService.frameReady += streamCommunicationService_frameReady;
            streamCommunicationService.participantBufferReady += streamCommunicationService_participantBufferReady;
         
        }

        /// <summary>
        ///re-throw event
        /// </summary>
        /// <param name="bitmaps">buffer of bitmaps</param>
        /// <param name="audio">byte[] of the audio</param>
        private void streamCommunicationService_participantBufferReady(List<Bitmap> bitmaps, List<byte[]> audio)
        {
            participantBufferReady(bitmaps, audio);
        }

        /// <summary>
        /// throw event when frame is ready
        /// </summary>
        /// <param name="bmp">bitmap</param>
        private void streamCommunicationService_frameReady(Bitmap bmp)
        {
            frameReady(bmp);
        }

        /// <summary>
        /// start capturing devices
        /// </summary>
        public void StartCapture()
        {
            streamCommunicationService.StartCapture();
        }

        /// <summary>
        /// stop capturing devices
        /// </summary>
        public void StopCapture()
        {
            streamCommunicationService.StopCapture();
        }

        /// <summary>
        /// when application ended
        /// </summary>
        public void On_Application_Ended()
        {
            streamCommunicationService.On_Application_Ended();
        }

        /// <summary>
        /// set the videomessage
        /// </summary>
        /// <param name="recipient">id of the recipient of the videomessage</param>
        public void SetVideoMessage(string recipient)
        {
            streamCommunicationService.SetVideoMessage(recipient);
        }
        /// <summary>
        /// get the videomessage form a specific recipient
        /// </summary>
        /// <param name="videoMessageId">id of the message to get</param>
        public void GetVideoMessage(string videoMessageId)
        {
            streamCommunicationService.GetVideoMessage(videoMessageId);
        }
    }
}
