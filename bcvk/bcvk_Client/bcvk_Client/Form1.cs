using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#region ManuallyAdded
using Thrift.Protocol;
using Thrift.Transport;
using bcvkSignal;
using bcvkStream;

using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;
#endregion

namespace bcvk_Client
{
    /// <summary>
    /// fhghfhghfghg
    /// </summary>
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
        #region Thrift classes
            //Thrift classes
            private TTransport transportSignal;
            private TProtocol protocolSignal;
            private TTransport transportStream;
            private TProtocol protocolStream;
            private Signal.Client signalClient;
            private bcvkStream.Stream.Client streamClient;
            private int signalPort = 9090;
            private int streamPort = 8080; 
        #endregion

        private CallState callState;
        private Webcam webcam;
        private Converter converter;
        private Buffer buffer;

        private string USERNAME;
        private List<byte[]> videoStreamBuffer;

        /// <summary>
        /// constructor
        /// </summary>
        public bcvk()
        {
            InitializeComponent();

            #region Declaration Thrift classes and tries to open transport
            try
            {
                //Signal settings: port 9090
                transportSignal = new TSocket("localhost", signalPort);
                protocolSignal = new TBinaryProtocol(transportSignal);
                //Stream settings: port 8080
                transportStream = new TSocket("localhost", streamPort);
                protocolStream = new TBinaryProtocol(transportStream);

                signalClient = new Signal.Client(protocolSignal);
                streamClient = new bcvkStream.Stream.Client(protocolStream);

                transportSignal.Open();
                transportStream.Open();
            }
            catch (System.Net.Sockets.SocketException exc)
            {
                MessageBox.Show(exc.Message);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            #endregion

            //TODO: Verkrijg contacten na inloggen
            //var accountData = signalClient.GetAccountData(USERNAME);

            webcam = new Webcam();
            webcam.frameReady += new Action<Bitmap>(FrameReadyHandler);

            converter = new Converter();
            buffer = new Buffer();
            buffer.bufferReady += buffer_bufferReady;
            SettingsCallState(CallState.CALL);
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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCallContact_Click(object sender, EventArgs e)
        {
            SettingsCallState(CallState.IS_CALLING);
            webcam.StartCamera();

            /*
             * TODO: connect to recipient
             * if(ontvanger positief beantwoord)
             * {
             *      CallState = CallState.IN_CALL;
             *      Alle andere dingen hier regelen!
             * }
             */
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
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// sets the correct settings to the enum
        /// </summary>
        /// <param name="callState">CallState enum</param>
        private void SettingsCallState(CallState callState)
        {
            this.callState = callState;
            //TODO: Enable this when users can be called!!!
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
            webcam.On_FormClosed();
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// enable webcam
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bcvk_Load(object sender, EventArgs e)
        {
            string message = webcam.On_Load();
            if (message != "")
                MessageBox.Show(message);
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// event will be triggered with each new available frame
        /// </summary>
        /// <param name="bmp">Bitmap of the frame</param>
        private void FrameReadyHandler(Bitmap bmp)
        {
            buffer.Buffer_VideoStream(converter.ToByteArray(bmp));
            pictureBoxVideoSend.BackgroundImage = bmp;
        }

        /// <summary>
        /// Luc Schnabel 1207776,
        /// this event is triggered when the bufferc  is ready
        /// </summary>
        /// <param name="videoStreamBuffer"></param>
        void buffer_bufferReady(List<byte[]> videoStreamBuffer)
        {
            //streamClient.send_SendStream();
        }
    }
}
