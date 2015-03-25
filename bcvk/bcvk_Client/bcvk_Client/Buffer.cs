using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcvk_Client
{
    public class Buffer
    {
        //TODO: create Singleton-object?

        public event Action< List<Byte[]> > bufferReady;

        public List<byte[]> videoStreamBuffer { get; private set; }

        //Constructor buffer videostream
        public void Buffer_VideoStream(byte[] bA)
        {
            //TODO: Create thread for non blocking code
            videoStreamBuffer.Add(bA);
            if (videoStreamBuffer.Count == 10)
                bufferReady(videoStreamBuffer);
        }

        //Constructor
        public Buffer()
        {
            videoStreamBuffer = new List<byte[]>();
        }
    }
}
