using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cc;
using System.Threading;

namespace bcvk_Client
{
    enum CallState
    {
        /// <summary>There is no current call. Calling is possible.</summary>
        CALL,
        /// <summary>User is calling someone. NOT YET RESPONDED BY RECIPIENT(S).</summary>
        IS_CALLING,
        /// <summary> User is calling with someone.</summary>
        IN_CALL
    }

    public partial class bcvk : Form
    {
        private CallState callState;
        private StreamControlClass streamControlClass;
        private SignalControlClass signalControlClass;

        private List<Bitmap>[] videoBufferArray = new List<Bitmap>[10];//this array can contain  elements of videobuffers
        private int readVideoBufferPointer = 0;
        private int addVideoBufferPointer = 0;
        private Thread drawThread;
        private bool readFromBegin;

        /// <summary>
        /// Constructor
        /// </summary>
        public bcvk()
        {
            InitializeComponent();
            SettingsCallState(CallState.CALL);

            signalControlClass = new SignalControlClass();
            
            //Start the thread of polling
            
            signalControlClass.beingCalled += signalControlClass_beingCalled;
            signalControlClass.initContacts += signalControlClass_initContacts;
            

            streamControlClass = new StreamControlClass();
            streamControlClass.frameReady += streamControlClass_frameReady;
            streamControlClass.participantBufferReady += streamControlClass_participantBufferReady;

            //start poll as last service so events have time to subcribe.
            signalControlClass.StartPoll();


        }

        /// <summary>
        /// participant buffer is ready to draw
        /// </summary>
        /// <param name="videostream">buffer of the videoframes</param>
        /// <param name="audio">byte[] of the audio</param>
        private void streamControlClass_participantBufferReady(List<Bitmap> videostream, List<byte[]> audio)
        {
            if(addVideoBufferPointer >= videoBufferArray.Length)
            {
                addVideoBufferPointer = 0;
                readFromBegin = true;
            }
            videoBufferArray[addVideoBufferPointer] = videostream;
            addVideoBufferPointer++;
            
            if(drawThread == null)
            {
                drawThread = new Thread(() => DrawParticipanBuffer());
                drawThread.Start();
            }
            
        }

        /// <summary>
        /// Draws the videobufferarray on the screen
        /// </summary>
        private void DrawParticipanBuffer()
        {
            while (true)
            {
                if (readVideoBufferPointer >= videoBufferArray.Length - 1)
                {
                    readVideoBufferPointer = 0;
                }
                else if(readVideoBufferPointer < videoBufferArray.Length)
                {
                    if (readVideoBufferPointer >= addVideoBufferPointer)
                    {
                        if (readFromBegin)
                        {
                            readVideoBufferPointer = 0;
                        }
                        else
                        {
                            Thread.Sleep(117);
                        }
                    }
                    else
                    {
                        foreach (Bitmap frame in videoBufferArray[readVideoBufferPointer])
                        {
                            pictureBoxVideoReceived.BackgroundImage = frame;
                            Thread.Sleep(117);
                        }
                        videoBufferArray[readVideoBufferPointer] = null;
                        GC.Collect();
                        readVideoBufferPointer++; 
                    }
                }
            }
        }

        /// <summary>
        /// Sets the contacts to the list
        /// </summary>
        private void signalControlClass_initContacts(string contacts)
        {
            //TODO: set contacts to list
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// triggered when this user gets called (a new connection)
        /// </summary>
        private void signalControlClass_beingCalled(string message)
        {
            if (MessageBox.Show(message, "Je wordt gebeld", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
            {
                //TODO: accept call, redo this.
                signalControlClass.AnswerCall(null, null, null, "connected");
            }
            else
            {
                //TODO: decline call
            }
        }

        /// <summary>
        /// Aron Huntjens 1209361, Luc Schnabel 1207776,
        /// event which is triggered when a new frame is ready
        /// </summary>
        /// <param name="bmp">Bitmap of the frame</param>
        private void streamControlClass_frameReady(Bitmap bmp)
        {
            pictureBoxVideoSend.BackgroundImage = new Bitmap(bmp, pictureBoxVideoSend.Width, pictureBoxVideoSend.Height);
        }

        /// <summary>
        /// Luc Schnabel 12077776,
        /// makes the video of the own camera (in)visible on the users own screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbShowYourVideo_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowYourVideo.Checked)
                pictureBoxVideoSend.Visible = true;
            else
                pictureBoxVideoSend.Visible = false;
        }
 
        /// <summary>
        /// Luc Schnabel 1207776,
        /// call the selected contact
        /// DISCLAIMER: (TODO:) THIS CODE BELONGS IN THE SELECT EVENT OF THE CONTACTLIST
        /// </summary>
        private void btnTestCall_Click(object sender, EventArgs e)
        {
            SettingsCallState(CallState.IS_CALLING);
            signalControlClass.DoCall("2");//listContacts.SelectedValue.ToString());
            streamControlClass.StartCapture();
            signalControlClass.AnswerCall(null, null, null, "connected");
        }

        /// <summary>
        /// Luc Schnabel 1207776
        /// End the extising call
        /// </summary>
        private void btnEndCall_Click(object sender, EventArgs e)
        {
            //SettingsCallState(CallState.CALL);
            //signalControlClass.EndCall();
            streamControlClass.StopCapture();
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// sets the correct settings to the enum
        /// </summary>
        /// <param name="callState">CallState enum</param>
        private void SettingsCallState(CallState callState)
        {
            this.callState = callState;
            ////TODO: Enable this when users can be called!!!
            //if (callState == CallState.CALL)
            //{
            //    btnCallContact.Visible = true; 
            //    btnEndCall.Visible = false;
            //}
            //if (callState == CallState.IS_CALLING)
            //{ 
            //    btnCallContact.Visible = false; 
            //    btnEndCall.Visible = false;
            //}
            //if (callState == CallState.IN_CALL)
            //{
            //    btnCallContact.Visible = false; 
            //    btnEndCall.Visible = true;
            //}
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// resize event to resize the form aspect ratio 701:461
        /// These numbers take care that the picturebox will be around aspect ratio 4:3
        /// This aspect ratio makes the image look good
        /// </summary>
        private void bcvk_Resize(object sender, EventArgs e)
        {
            int currentHeight, currentWidth, correctedHeight, correctedWith;
            currentHeight = this.Height;
            currentWidth = this.Width;
            
            correctedHeight = (currentWidth * 461) / 701;
            if (correctedHeight > currentHeight)
            {
                correctedWith = (currentHeight * 701) / 461;

                this.Height = currentHeight;
                this.Width = correctedWith;
            }
            else
            {
                this.Height = correctedHeight;
                this.Width = currentWidth;
            }
            double ratio = Convert.ToDouble(pictureBoxVideoReceived.Width) / Convert.ToDouble(pictureBoxVideoReceived.Height);
            double formRatio = Convert.ToDouble(this.Width) / Convert.ToDouble(this.Height);
        }

        /// <summary>
        /// Aron Huntjens 1209361,
        /// call the selected contact
        /// </summary>
        private void listContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: signalControlClass.DoCall(listContacts.SelectedValue.ToString());
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// stops and free objects if application is closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bcvk_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
            //streamControlClass.On_Application_Ended();
            //drawThread.Abort();
            //if (!signalControlClass.StopPoll())
            //    e.Cancel = true;
        }
    }
}
