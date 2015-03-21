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
#endregion

namespace bcvk_Client
{
    public partial class bcvk : Form
    {
        private Webcam webcam;

        public bcvk()
        {
            InitializeComponent();
            webcam = new Webcam(pictureBoxVideoSend);
        }

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

        private void cbShowYourVideo_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowYourVideo.Checked)
                pictureBoxVideoSend.Visible = true;
            else
                pictureBoxVideoSend.Visible = false;
        }
    }
}
