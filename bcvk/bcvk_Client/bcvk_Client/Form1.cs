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
#endregion

namespace bcvk_Client
{
    public partial class bcvk : Form
    {
        //create webcam class
        private Webcam webcam;
        private ConvertToFromByteArray toFromByteArray;
        private List<byte[]> buffer;

        #region Thrift classes
        //Thrift classes
        private TTransport transportSignal;
        private TProtocol protocolSignal;
        private TTransport transportStream;
        private TProtocol protocolStream;
        private Signal.Client signalClient;
        private Stream.Client streamClient;
        private int signalPort = 9090;
        private int streamPort = 8080; 
        #endregion

        //constructor
        public bcvk()
        {
            InitializeComponent();

            webcam = new Webcam();
            toFromByteArray = new ConvertToFromByteArray();
            webcam.frameReady += new Action<Bitmap>(FrameReadyHandler);
            buffer = new List<byte[]>();

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
                streamClient = new Stream.Client(protocolStream);

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
        }

        private void FrameReadyHandler(Bitmap bmp)
        {
            buffer.Add(toFromByteArray.BitmapToByteArray(bmp));

            //TODO: Zet frames in een buffer
            //TODO: Verstuur hier eerste byte array in de buffer naar de server
            
            //TODO: VOORBEELD
            //streamClient.send_SendStream("", "", buffer[0]);
            
            pictureBoxVideoSend.BackgroundImage = bmp;
        }

        /// <summary>
        /// this event is triggered when the form is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            string message = webcam.On_Load();
            if (message != "")
                MessageBox.Show(message);
        }

        private void btnStartCamera_Click(object sender, EventArgs e)
        {
            webcam.btnStartCamera();
        }

        private void btnStopCamera_Click(object sender, EventArgs e)
        {
            webcam.btnStopCamera();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            webcam.On_FormClosed();
        }

        /// <summary>
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

    }
}
