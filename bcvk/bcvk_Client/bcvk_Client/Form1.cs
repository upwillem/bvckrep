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
        /// constructor
        /// </summary>
        public bcvk()
        {
            InitializeComponent();
            SettingsCallState(CallState.CALL);

            signalControlClass = new SignalControlClass("pietje"/*testusername (subaccount)*/);
            signalControlClass.StartPoll();
            signalControlClass.beingCalledEvent += signalControlClass_beingCalledEvent;
            streamControlClass = new StreamControlClass();
            streamControlClass.frameReady += streamControlClass_frameReady;

            InitContacts();
        }

        /// <summary>
        /// Sets the contacts to the list
        /// </summary>
        private void InitContacts()
        {
            
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// triggered when this user gets called (a new connection)
        /// </summary>
        /// <param name="obj"></param>
        private void signalControlClass_beingCalledEvent(string message)
        {
            if (MessageBox.Show(message, "Je bent niet vriendenloos :D", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
            {

                //TODO: accept call
            }
        }

        /// <summary>
        /// Event which is triggered when a new frame is ready
        /// </summary>
        /// <param name="bmp"></param>
        private void streamControlClass_frameReady(Bitmap bmp)
        {
            pictureBoxVideoSend.BackgroundImage = bmp;
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

        private void listContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            signalControlClass.DoCall(listContacts.SelectedValue.ToString());
        }
    }
}
