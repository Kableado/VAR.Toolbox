namespace VAR.Toolbox.UI
{
    partial class FrmScreenshooter
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
            this.btnScreenshoot = new System.Windows.Forms.Button();
            this.picViewer = new VAR.Toolbox.Controls.CtrImageViewer();
            this.btnStartStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // btnScreenshoot
            // 
            this.btnScreenshoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnScreenshoot.Location = new System.Drawing.Point(12, 390);
            this.btnScreenshoot.Name = "btnScreenshoot";
            this.btnScreenshoot.Size = new System.Drawing.Size(75, 23);
            this.btnScreenshoot.TabIndex = 1;
            this.btnScreenshoot.Text = "Screenshoot";
            this.btnScreenshoot.UseVisualStyleBackColor = true;
            this.btnScreenshoot.Click += new System.EventHandler(this.btnScreenshoot_Click);
            // 
            // picViewer
            // 
            this.picViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picViewer.BackColor = System.Drawing.Color.Black;
            this.picViewer.ImageShow = null;
            this.picViewer.Location = new System.Drawing.Point(13, 13);
            this.picViewer.Name = "picViewer";
            this.picViewer.Size = new System.Drawing.Size(580, 371);
            this.picViewer.TabIndex = 0;
            this.picViewer.TabStop = false;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStartStop.Location = new System.Drawing.Point(93, 390);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 23);
            this.btnStartStop.TabIndex = 2;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // FrmScreenshooter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 425);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.btnScreenshoot);
            this.Controls.Add(this.picViewer);
            this.Name = "FrmScreenshooter";
            this.Text = "FrmScreenshooter";
            ((System.ComponentModel.ISupportInitialize)(this.picViewer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VAR.Toolbox.Controls.CtrImageViewer picViewer;
        private System.Windows.Forms.Button btnScreenshoot;
        private System.Windows.Forms.Button btnStartStop;
    }
}