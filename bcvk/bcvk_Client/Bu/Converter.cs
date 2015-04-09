using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bu
{
    public class Converter
    {
        private ImageConverter converter;

        //constructor
        public Converter()
        {
            converter = new ImageConverter();
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// converts Image to byte[]
        /// </summary>
        /// <param name="img">image to convert</param>
        public byte[] ToByteArray(Image img)
        {
            byte[] bA = (byte[])converter.ConvertTo(img, typeof(byte[]));
            return bA;
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// converts byte[] to bitmap
        /// </summary>
        /// <param name="bA">byte array to convert</param>
        public Bitmap ToBitmap(byte[] bA, int width, int height)
        {
            Bitmap bmp = new Bitmap((Bitmap)converter.ConvertFrom(bA), width, height);
            return bmp;
        }
    }
}
