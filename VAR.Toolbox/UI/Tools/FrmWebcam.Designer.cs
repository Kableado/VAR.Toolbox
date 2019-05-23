namespace VAR.Toolbox.UI
{
    partial class FrmWebcam
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
            this.btnStartStop = new System.Windows.Forms.Button();
            this.cboWebcams = new System.Windows.Forms.ComboBox();
            this.picWebcam = new VAR.Toolbox.Controls.CtrImageViewer();
            ((System.ComponentModel.ISupportInitialize)(this.picWebcam)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartStop
            // 
            this.btnStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStartStop.Location = new System.Drawing.Point(18, 500);
            this.btnStartStop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(112, 35);
            this.btnStartStop.TabIndex = 4;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // cboWebcams
            // 
            this.cboWebcams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboWebcams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWebcams.FormattingEnabled = true;
            this.cboWebcams.Location = new System.Drawing.Point(140, 500);
            this.cboWebcams.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboWebcams.Name = "cboWebcams";
            this.cboWebcams.Size = new System.Drawing.Size(397, 28);
            this.cboWebcams.TabIndex = 5;
            // 
            // picWebcam
            // 
            this.picWebcam.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picWebcam.BackColor = System.Drawing.Color.Black;
            this.picWebcam.ImageShow = null;
            this.picWebcam.Location = new System.Drawing.Point(18, 18);
            this.picWebcam.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picWebcam.Name = "picWebcam";
            this.picWebcam.Size = new System.Drawing.Size(520, 472);
            this.picWebcam.TabIndex = 0;
            this.picWebcam.TabStop = false;
            // 
            // FrmWebcam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 554);
            this.Controls.Add(this.cboWebcams);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.picWebcam);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmWebcam";
            this.Text = "Webcam";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmWebcam_FormClosed);
            this.Load += new System.EventHandler(this.FrmWebcam_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picWebcam)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VAR.Toolbox.Controls.CtrImageViewer picWebcam;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.ComboBox cboWebcams;
    }
}