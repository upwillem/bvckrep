using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#region Manually added
using Thrift.Protocol;
using Thrift.Transport;
using AForge.Video;
using AForge.Video.DirectShow;
#endregion

namespace bcvk_Client
{
    public partial class bcvk : Form
    {
        private VideoCaptureDevice videoSource = new VideoCaptureDevice();
        public bcvk()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //List all available video sources. (That can be webcams as well as tv cards, etc)
            FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //Check if atleast one video source is available
            
            if (videosources != null)
            {
                //For example use first video device. You may check if this is your webcam.
                videoSource = new VideoCaptureDevice(videosources[0].MonikerString);

                int height = pictureBoxVideo.Height;
                int width = pictureBoxVideo.Width;

                try
                {
                    //Check if the video device provides a list of supported resolutions
                    if (videoSource.VideoCapabilities.Length > 0)
                    {
                        string highestSolution = "0;0";
                        //Search for the highest resolution
                        for (int i = 0; i < videoSource.VideoCapabilities.Length; i++)
                        {
                            if (videoSource.VideoCapabilities[i].FrameSize.Width <= pictureBoxVideo.Width)
                            {
                                highestSolution = videoSource.VideoCapabilities[i].FrameSize.Width.ToString() + ";" + i.ToString();
                                height = videoSource.VideoCapabilities[i].FrameSize.Height;
                                width = videoSource.VideoCapabilities[i].FrameSize.Width;
                            }
                        }

                        pictureBoxVideo.Width = width;
                        pictureBoxVideo.Height = height;

                        //Set the highest resolution as active
                        videoSource.VideoResolution = videoSource.VideoCapabilities[Convert.ToInt32(highestSolution.Split(';')[1])];
                    }
                }
                catch (Exception Exception)
                {
                    MessageBox.Show(Exception.Message);
                }

                //Create NewFrame event handler
                //(This one triggers every time a new frame/image is captured)
                videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame);
            }
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //Cast the frame as Bitmap object and don't forget to use ".Clone()" otherwise
            //you'll probably get access violation exceptions
            pictureBoxVideo.BackgroundImage = (Bitmap)eventArgs.Frame.Clone();
            
        } 

        private void btnStartCamera_Click(object sender, EventArgs e)
        { 
            // start the video source
            videoSource.Start( );
        }

        private void btnStopCamera_Click(object sender, EventArgs e)
        {
            // signal to stop when you no longer need capturing
            videoSource.SignalToStop();
            pictureBoxVideo.BackColor = Color.Transparent;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
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
