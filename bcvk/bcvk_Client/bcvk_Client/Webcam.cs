using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region ManuallyAdded
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace bcvk_Client
{
    public class Webcam
    {
        private VideoCaptureDevice videoSource = new VideoCaptureDevice();
        private PictureBox pictureBoxVideoSend;

        public Webcam(PictureBox p)
        {
            pictureBoxVideoSend = p;
        }

        public string On_Load()
        {
            //List all available video sources. (That can be webcams as well as tv cards, etc)
            FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //Check if atleast one video source is available
            if (videosources != null)
            {
                //For example use first video device. You may check if this is your webcam.
                videoSource = new VideoCaptureDevice(videosources[0].MonikerString);

                try
                {
                    //Check if the video device provides a list of supported resolutions
                    if (videoSource.VideoCapabilities.Length > 0)
                    {
                        string highestSolution = "0;0";
                        //Search for the highest resolution
                        for (int i = 0; i < videoSource.VideoCapabilities.Length; i++)
                        {
                            if (videoSource.VideoCapabilities[i].FrameSize.Width <= 160)
                                highestSolution = videoSource.VideoCapabilities[i].FrameSize.Width.ToString() + ";" + i.ToString();
                        }

                        //Set the highest resolution as active
                        videoSource.VideoResolution = videoSource.VideoCapabilities[Convert.ToInt32(highestSolution.Split(';')[1])];
                    }
                }
                catch (Exception Exception)
                {
                    return Exception.Message;
                }

                //Create NewFrame event handler
                //(This one triggers every time a new frame/image is captured)
                videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame);  
            }
            return "";
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //Cast the frame as Bitmap object and don't forget to use ".Clone()" otherwise
            //you'll probably get access violation exceptions
            pictureBoxVideoSend.BackgroundImage = (Bitmap)eventArgs.Frame.Clone();
            
        } 

        public void btnStartCamera()
        { 
            // start the video source
            videoSource.Start( );
        }

        public void btnStopCamera()
        {
            // signal to stop when you no longer need capturing
            videoSource.SignalToStop();
        }

        public void On_FormClosed()
        {
            //Stop and free the webcam object if application is closing
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource = null;
            }
        }
    }
}
