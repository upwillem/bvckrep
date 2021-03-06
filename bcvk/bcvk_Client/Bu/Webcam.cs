﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region ManuallyAdded
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
#endregion

namespace Bu
{
    /// <summary>
    /// Owner: Luc Schnabel 1207776
    /// </summary>
    public class Webcam
    {
        private VideoCaptureDevice videoSource;
        public event Action<Bitmap> frameReady;

        /// <summary>
        /// Constructor
        /// </summary>
        public Webcam()
        { 
            videoSource = new VideoCaptureDevice();
            SetResolution();
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// looks for a videosource with a resolution of 160 by 120
        /// and connects the frame event
        /// </summary>
        /// <returns>string error</returns>
        private string SetResolution()
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
                            if ((videoSource.VideoCapabilities[i].FrameSize.Width >= highestSolution[0]) && (videoSource.VideoCapabilities[i].FrameSize.Width <= 320 ))
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
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);  
            }
            return "";
        }

        /// <summary>
        /// Luc Schnabel 1207776, 
        /// triggers the event to tell the form that a new fram (bitmap) is ready
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            frameReady((Bitmap)eventArgs.Frame.Clone());
        } 

        /// <summary>
        /// Luc Schnabel 1207776, 
        /// start the camera
        /// </summary>
        public void Start()
        {
            videoSource.Start( );
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// stop the camera.
        /// Signal to stop when you no longer need capturing
        /// </summary>
        public void Stop()
        {
            videoSource.SignalToStop();
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// stops and free the webcam object if application is closing
        /// </summary>
        public void On_Application_End()
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
