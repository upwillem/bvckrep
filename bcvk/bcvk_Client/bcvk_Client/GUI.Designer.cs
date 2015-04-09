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
            this.pictureBoxVideoSend = new System.Windows.Forms.PictureBox();
            this.btnTestCall = new System.Windows.Forms.Button();
            this.btnEndCall = new System.Windows.Forms.Button();
            this.cbShowYourVideo = new System.Windows.Forms.CheckBox();
            this.pictureBoxVideoReceived = new System.Windows.Forms.PictureBox();
            this.listContacts = new System.Windows.Forms.ListBox();
            this.labelContacten = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideoSend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideoReceived)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxVideoSend
            // 
            this.pictureBoxVideoSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxVideoSend.Location = new System.Drawing.Point(513, 291);
            this.pictureBoxVideoSend.Name = "pictureBoxVideoSend";
            this.pictureBoxVideoSend.Size = new System.Drawing.Size(160, 120);
            this.pictureBoxVideoSend.TabIndex = 0;
            this.pictureBoxVideoSend.TabStop = false;
            // 
            // btnTestCall
            // 
            this.btnTestCall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTestCall.Location = new System.Drawing.Point(12, 325);
            this.btnTestCall.Name = "btnTestCall";
            this.btnTestCall.Size = new System.Drawing.Size(123, 23);
            this.btnTestCall.TabIndex = 1;
            this.btnTestCall.Text = "Test bel";
            this.btnTestCall.UseVisualStyleBackColor = true;
            this.btnTestCall.Click += new System.EventHandler(this.btnTestCall_Click);
            // 
            // btnEndCall
            // 
            this.btnEndCall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEndCall.Location = new System.Drawing.Point(12, 354);
            this.btnEndCall.Name = "btnEndCall";
            this.btnEndCall.Size = new System.Drawing.Size(123, 23);
            this.btnEndCall.TabIndex = 2;
            this.btnEndCall.Text = "Stop gesprek";
            this.btnEndCall.UseVisualStyleBackColor = true;
            this.btnEndCall.Click += new System.EventHandler(this.btnEndCall_Click);
            // 
            // cbShowYourVideo
            // 
            this.cbShowYourVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbShowYourVideo.AutoSize = true;
            this.cbShowYourVideo.Checked = true;
            this.cbShowYourVideo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowYourVideo.Location = new System.Drawing.Point(12, 394);
            this.cbShowYourVideo.Name = "cbShowYourVideo";
            this.cbShowYourVideo.Size = new System.Drawing.Size(120, 17);
            this.cbShowYourVideo.TabIndex = 3;
            this.cbShowYourVideo.Text = "Toon je eigen beeld";
            this.cbShowYourVideo.UseVisualStyleBackColor = true;
            this.cbShowYourVideo.CheckedChanged += new System.EventHandler(this.cbShowYourVideo_CheckedChanged);
            // 
            // pictureBoxVideoReceived
            // 
            this.pictureBoxVideoReceived.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxVideoReceived.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBoxVideoReceived.Location = new System.Drawing.Point(141, 12);
            this.pictureBoxVideoReceived.Name = "pictureBoxVideoReceived";
            this.pictureBoxVideoReceived.Size = new System.Drawing.Size(532, 399);
            this.pictureBoxVideoReceived.TabIndex = 4;
            this.pictureBoxVideoReceived.TabStop = false;
            // 
            // listContacts
            // 
            this.listContacts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listContacts.FormattingEnabled = true;
            this.listContacts.Location = new System.Drawing.Point(12, 38);
            this.listContacts.Name = "listContacts";
            this.listContacts.Size = new System.Drawing.Size(123, 277);
            this.listContacts.TabIndex = 5;
            this.listContacts.SelectedIndexChanged += new System.EventHandler(this.listContacts_SelectedIndexChanged);
            // 
            // labelContacten
            // 
            this.labelContacten.AutoSize = true;
            this.labelContacten.Location = new System.Drawing.Point(13, 17);
            this.labelContacten.Name = "labelContacten";
            this.labelContacten.Size = new System.Drawing.Size(63, 13);
            this.labelContacten.TabIndex = 6;
            this.labelContacten.Text = "Vriendenlijst";
            // 
            // bcvk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 423);
            this.Controls.Add(this.btnTestCall);
            this.Controls.Add(this.labelContacten);
            this.Controls.Add(this.pictureBoxVideoSend);
            this.Controls.Add(this.listContacts);
            this.Controls.Add(this.pictureBoxVideoReceived);
            this.Controls.Add(this.cbShowYourVideo);
            this.Controls.Add(this.btnEndCall);
            this.Name = "bcvk";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Communiceren voor kinderen";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.bcvk_FormClosing);
            this.Resize += new System.EventHandler(this.bcvk_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideoSend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideoReceived)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxVideoSend;
        private System.Windows.Forms.Button btnTestCall;
        private System.Windows.Forms.Button btnEndCall;
        private System.Windows.Forms.CheckBox cbShowYourVideo;
        private System.Windows.Forms.PictureBox pictureBoxVideoReceived;
        private System.Windows.Forms.ListBox listContacts;
        private System.Windows.Forms.Label labelContacten;
    }
}

