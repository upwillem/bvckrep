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

        /// <summary>
        /// Constructor
        /// </summary>
        public bcvk()
        {
            InitializeComponent();
            SettingsCallState(CallState.CALL);

            signalControlClass = new SignalControlClass("pietje"/*testusername (subaccount)*/);
            //Start the thread of polling
            signalControlClass.StartPoll();
            signalControlClass.beingCalled += signalControlClass_beingCalled;
            signalControlClass.initContacts += signalControlClass_initContacts;

            streamControlClass = new StreamControlClass();
            streamControlClass.frameReady += streamControlClass_frameReady;

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
                //TODO: accept call
                signalControlClass.AnswerCall(signalControlClass.Username, signalControlClass.Username, signalControlClass.ConnectionId, "connected");
            }
            else
            {
                //TODO: decline call
                signalControlClass.AnswerCall(signalControlClass.Username, "", signalControlClass.ConnectionId, "disconnected");
            }
        }

        /// <summary>
        /// Event which is triggered when a new frame is ready
        /// </summary>
        /// <param name="bmp"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestCall_Click(object sender, EventArgs e)
        {
            SettingsCallState(CallState.IS_CALLING);
            signalControlClass.DoCall("3");//listContacts.SelectedValue.ToString());
            streamControlClass.StartCamera();
        }

        /// <summary>
        /// Luc Schnabel 1207776
        /// End the extising call
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEndCall_Click(object sender, EventArgs e)
        {
            SettingsCallState(CallState.CALL);
            streamControlClass.StopCamera();
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
        /// stops and free objects if application is closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bcvk_FormClosed(object sender, FormClosedEventArgs e)
        {
            signalControlClass.PollThread.Abort();
            streamControlClass.On_Application_Ended();
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
        /// Call the selected contact
        /// </summary>
        private void listContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            signalControlClass.DoCall(listContacts.SelectedValue.ToString());
        }
    }
}
