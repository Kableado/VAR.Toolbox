namespace VAR.Toolbox.UI
{
    partial class FrmToolbox
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
            this.components = new System.ComponentModel.Container();
            this.btnBase64 = new System.Windows.Forms.Button();
            this.btnProxyCmd = new System.Windows.Forms.Button();
            this.btnWebcam = new System.Windows.Forms.Button();
            this.btnTunnelTCP = new System.Windows.Forms.Button();
            this.lblToolbox = new System.Windows.Forms.Label();
            this.pnlSuspension1 = new VAR.Toolbox.UI.PnlSuspension();
            this.pnlCover1 = new VAR.Toolbox.UI.PnlCover();
            this.niTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnExit = new System.Windows.Forms.Button();
            this.btnTestSoapService = new System.Windows.Forms.Button();
            this.btnTestRestService = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBase64
            // 
            this.btnBase64.Location = new System.Drawing.Point(9, 52);
            this.btnBase64.Name = "btnBase64";
            this.btnBase64.Size = new System.Drawing.Size(165, 34);
            this.btnBase64.TabIndex = 0;
            this.btnBase64.Text = "Base64";
            this.btnBase64.UseVisualStyleBackColor = true;
            this.btnBase64.Click += new System.EventHandler(this.btnBase64_Click);
            // 
            // btnProxyCmd
            // 
            this.btnProxyCmd.Location = new System.Drawing.Point(9, 92);
            this.btnProxyCmd.Name = "btnProxyCmd";
            this.btnProxyCmd.Size = new System.Drawing.Size(165, 36);
            this.btnProxyCmd.TabIndex = 1;
            this.btnProxyCmd.Text = "ProxyCmd";
            this.btnProxyCmd.UseVisualStyleBackColor = true;
            this.btnProxyCmd.Click += new System.EventHandler(this.btnProxyCmd_Click);
            // 
            // btnWebcam
            // 
            this.btnWebcam.Location = new System.Drawing.Point(9, 135);
            this.btnWebcam.Name = "btnWebcam";
            this.btnWebcam.Size = new System.Drawing.Size(165, 36);
            this.btnWebcam.TabIndex = 2;
            this.btnWebcam.Text = "Webcam";
            this.btnWebcam.UseVisualStyleBackColor = true;
            this.btnWebcam.Click += new System.EventHandler(this.btnWebcam_Click);
            // 
            // btnTunnelTCP
            // 
            this.btnTunnelTCP.Location = new System.Drawing.Point(9, 175);
            this.btnTunnelTCP.Margin = new System.Windows.Forms.Padding(2);
            this.btnTunnelTCP.Name = "btnTunnelTCP";
            this.btnTunnelTCP.Size = new System.Drawing.Size(165, 36);
            this.btnTunnelTCP.TabIndex = 5;
            this.btnTunnelTCP.Text = "TunnelTCP";
            this.btnTunnelTCP.UseVisualStyleBackColor = true;
            this.btnTunnelTCP.Click += new System.EventHandler(this.btnTunnelTCP_Click);
            // 
            // lblToolbox
            // 
            this.lblToolbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblToolbox.Font = new System.Drawing.Font("Arial Narrow", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToolbox.Location = new System.Drawing.Point(9, 9);
            this.lblToolbox.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblToolbox.Name = "lblToolbox";
            this.lblToolbox.Size = new System.Drawing.Size(336, 40);
            this.lblToolbox.TabIndex = 6;
            this.lblToolbox.Text = "Toolbox";
            this.lblToolbox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlSuspension1
            // 
            this.pnlSuspension1.Location = new System.Drawing.Point(180, 241);
            this.pnlSuspension1.Margin = new System.Windows.Forms.Padding(1);
            this.pnlSuspension1.Name = "pnlSuspension1";
            this.pnlSuspension1.Size = new System.Drawing.Size(165, 114);
            this.pnlSuspension1.TabIndex = 4;
            // 
            // pnlCover1
            // 
            this.pnlCover1.Location = new System.Drawing.Point(12, 241);
            this.pnlCover1.Margin = new System.Windows.Forms.Padding(1);
            this.pnlCover1.Name = "pnlCover1";
            this.pnlCover1.Size = new System.Drawing.Size(162, 114);
            this.pnlCover1.TabIndex = 3;
            // 
            // niTray
            // 
            this.niTray.Text = "VAR.Toolbox";
            this.niTray.Visible = true;
            this.niTray.MouseClick += new System.Windows.Forms.MouseEventHandler(this.niTray_MouseClick);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(12, 359);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(333, 29);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnTestSoapService
            // 
            this.btnTestSoapService.Location = new System.Drawing.Point(180, 52);
            this.btnTestSoapService.Name = "btnTestSoapService";
            this.btnTestSoapService.Size = new System.Drawing.Size(165, 34);
            this.btnTestSoapService.TabIndex = 8;
            this.btnTestSoapService.Text = "TestSoapService";
            this.btnTestSoapService.UseVisualStyleBackColor = true;
            this.btnTestSoapService.Click += new System.EventHandler(this.btnTestSoapService_Click);
            // 
            // btnTestRestService
            // 
            this.btnTestRestService.Location = new System.Drawing.Point(180, 92);
            this.btnTestRestService.Name = "btnTestRestService";
            this.btnTestRestService.Size = new System.Drawing.Size(165, 36);
            this.btnTestRestService.TabIndex = 9;
            this.btnTestRestService.Text = "TestRestService";
            this.btnTestRestService.UseVisualStyleBackColor = true;
            this.btnTestRestService.Click += new System.EventHandler(this.btnTestRestService_Click);
            // 
            // FrmToolbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 400);
            this.Controls.Add(this.btnTestRestService);
            this.Controls.Add(this.btnTestSoapService);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblToolbox);
            this.Controls.Add(this.btnTunnelTCP);
            this.Controls.Add(this.pnlSuspension1);
            this.Controls.Add(this.pnlCover1);
            this.Controls.Add(this.btnWebcam);
            this.Controls.Add(this.btnProxyCmd);
            this.Controls.Add(this.btnBase64);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "FrmToolbox";
            this.Text = "Toolbox";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmToolbox_FormClosing);
            this.Load += new System.EventHandler(this.FrmToolbox_Load);
            this.Resize += new System.EventHandler(this.FrmToolbox_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBase64;
        private System.Windows.Forms.Button btnProxyCmd;
        private System.Windows.Forms.Button btnWebcam;
        private PnlCover pnlCover1;
        private PnlSuspension pnlSuspension1;
        private System.Windows.Forms.Button btnTunnelTCP;
        private System.Windows.Forms.Label lblToolbox;
        private System.Windows.Forms.NotifyIcon niTray;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnTestSoapService;
        private System.Windows.Forms.Button btnTestRestService;
    }
}

