namespace bcvk_Client
{
    partial class bcvk
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxVideo = new System.Windows.Forms.PictureBox();
            this.btnStartCamera = new System.Windows.Forms.Button();
            this.btnStopCamera = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxVideo
            // 
            this.pictureBoxVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxVideo.Location = new System.Drawing.Point(739, 324);
            this.pictureBoxVideo.Name = "pictureBoxVideo";
            this.pictureBoxVideo.Size = new System.Drawing.Size(160, 120);
            this.pictureBoxVideo.TabIndex = 0;
            this.pictureBoxVideo.TabStop = false;
            // 
            // btnStartCamera
            // 
            this.btnStartCamera.Location = new System.Drawing.Point(13, 13);
            this.btnStartCamera.Name = "btnStartCamera";
            this.btnStartCamera.Size = new System.Drawing.Size(123, 23);
            this.btnStartCamera.TabIndex = 1;
            this.btnStartCamera.Text = "Start camera";
            this.btnStartCamera.UseVisualStyleBackColor = true;
            this.btnStartCamera.Click += new System.EventHandler(this.btnStartCamera_Click);
            // 
            // btnStopCamera
            // 
            this.btnStopCamera.Location = new System.Drawing.Point(13, 43);
            this.btnStopCamera.Name = "btnStopCamera";
            this.btnStopCamera.Size = new System.Drawing.Size(123, 23);
            this.btnStopCamera.TabIndex = 2;
            this.btnStopCamera.Text = "Stop camera";
            this.btnStopCamera.UseVisualStyleBackColor = true;
            this.btnStopCamera.Click += new System.EventHandler(this.btnStopCamera_Click);
            // 
            // bcvk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 456);
            this.Controls.Add(this.btnStopCamera);
            this.Controls.Add(this.btnStartCamera);
            this.Controls.Add(this.pictureBoxVideo);
            this.Name = "bcvk";
            this.Text = "Soort Van Skype Maar Niet Helemaal Hetzelfde";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxVideo;
        private System.Windows.Forms.Button btnStartCamera;
        private System.Windows.Forms.Button btnStopCamera;
    }
}

