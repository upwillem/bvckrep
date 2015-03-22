using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcvk_Client
{
    public class ConvertToFromByteArray
    {
        public ConvertToFromByteArray() { }

        /// <summary>
        /// Converts bitmap to byte[]
        /// </summary>
        /// <param name="bmp">bitmap of the frame</param>
        /// <returns>byte[] of the bitmap</returns>
        public byte[] BitmapToByteArray(Bitmap bmp)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        public Bitmap ByteArrayToBitmap(byte[] byteArray)
        {
            throw new NotImplementedException();
        }
    }
}
