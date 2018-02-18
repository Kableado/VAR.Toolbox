namespace VAR.Toolbox.UI
{
    partial class FrmTunnelTCP
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
            this.btnRun = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblRemoteHost = new System.Windows.Forms.Label();
            this.lblRemotePort = new System.Windows.Forms.Label();
            this.lblLocalPort = new System.Windows.Forms.Label();
            this.txtRemoteHost = new System.Windows.Forms.TextBox();
            this.txtRemotePort = new System.Windows.Forms.TextBox();
            this.txtLocalPort = new System.Windows.Forms.TextBox();
            this.ctrOutput = new VAR.Toolbox.Controls.CtrOutput();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(356, 368);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(83, 45);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(275, 368);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 45);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblRemoteHost
            // 
            this.lblRemoteHost.AutoSize = true;
            this.lblRemoteHost.Location = new System.Drawing.Point(12, 15);
            this.lblRemoteHost.Name = "lblRemoteHost";
            this.lblRemoteHost.Size = new System.Drawing.Size(100, 20);
            this.lblRemoteHost.TabIndex = 2;
            this.lblRemoteHost.Text = "RemoteHost";
            // 
            // lblRemotePort
            // 
            this.lblRemotePort.AutoSize = true;
            this.lblRemotePort.Location = new System.Drawing.Point(12, 47);
            this.lblRemotePort.Name = "lblRemotePort";
            this.lblRemotePort.Size = new System.Drawing.Size(95, 20);
            this.lblRemotePort.TabIndex = 3;
            this.lblRemotePort.Text = "RemotePort";
            // 
            // lblLocalPort
            // 
            this.lblLocalPort.AutoSize = true;
            this.lblLocalPort.Location = new System.Drawing.Point(12, 79);
            this.lblLocalPort.Name = "lblLocalPort";
            this.lblLocalPort.Size = new System.Drawing.Size(76, 20);
            this.lblLocalPort.TabIndex = 4;
            this.lblLocalPort.Text = "LocalPort";
            // 
            // txtRemoteHost
            // 
            this.txtRemoteHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemoteHost.Location = new System.Drawing.Point(118, 12);
            this.txtRemoteHost.Name = "txtRemoteHost";
            this.txtRemoteHost.Size = new System.Drawing.Size(321, 26);
            this.txtRemoteHost.TabIndex = 5;
            // 
            // txtRemotePort
            // 
            this.txtRemotePort.Location = new System.Drawing.Point(118, 44);
            this.txtRemotePort.Name = "txtRemotePort";
            this.txtRemotePort.Size = new System.Drawing.Size(100, 26);
            this.txtRemotePort.TabIndex = 6;
            // 
            // txtLocalPort
            // 
            this.txtLocalPort.Location = new System.Drawing.Point(118, 76);
            this.txtLocalPort.Name = "txtLocalPort";
            this.txtLocalPort.Size = new System.Drawing.Size(100, 26);
            this.txtLocalPort.TabIndex = 7;
            // 
            // lsbOutput
            // 
            this.ctrOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrOutput.Location = new System.Drawing.Point(12, 108);
            this.ctrOutput.Name = "lsbOutput";
            this.ctrOutput.Size = new System.Drawing.Size(427, 244);
            this.ctrOutput.TabIndex = 8;
            // 
            // FrmTunnelTCP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 425);
            this.Controls.Add(this.ctrOutput);
            this.Controls.Add(this.txtLocalPort);
            this.Controls.Add(this.txtRemotePort);
            this.Controls.Add(this.txtRemoteHost);
            this.Controls.Add(this.lblLocalPort);
            this.Controls.Add(this.lblRemotePort);
            this.Controls.Add(this.lblRemoteHost);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnRun);
            this.MinimumSize = new System.Drawing.Size(473, 372);
            this.Name = "FrmTunnelTCP";
            this.Text = "TunnelTCP";
            this.Load += new System.EventHandler(this.FrmTunnelTCP_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblRemoteHost;
        private System.Windows.Forms.Label lblRemotePort;
        private System.Windows.Forms.Label lblLocalPort;
        private System.Windows.Forms.TextBox txtRemoteHost;
        private System.Windows.Forms.TextBox txtRemotePort;
        private System.Windows.Forms.TextBox txtLocalPort;
        private VAR.Toolbox.Controls.CtrOutput ctrOutput;
    }
}