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
            this.btnBase64 = new System.Windows.Forms.Button();
            this.btnProxyCmd = new System.Windows.Forms.Button();
            this.btnWebcam = new System.Windows.Forms.Button();
            this.btnTunnelTCP = new System.Windows.Forms.Button();
            this.lblToolbox = new System.Windows.Forms.Label();
            this.pnlSuspension1 = new VAR.Toolbox.UI.PnlSuspension();
            this.pnlCover1 = new VAR.Toolbox.UI.PnlCover();
            this.SuspendLayout();
            // 
            // btnBase64
            // 
            this.btnBase64.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBase64.Location = new System.Drawing.Point(13, 80);
            this.btnBase64.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBase64.Name = "btnBase64";
            this.btnBase64.Size = new System.Drawing.Size(238, 52);
            this.btnBase64.TabIndex = 0;
            this.btnBase64.Text = "Base64";
            this.btnBase64.UseVisualStyleBackColor = true;
            this.btnBase64.Click += new System.EventHandler(this.btnBase64_Click);
            // 
            // btnProxyCmd
            // 
            this.btnProxyCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProxyCmd.Location = new System.Drawing.Point(13, 142);
            this.btnProxyCmd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnProxyCmd.Name = "btnProxyCmd";
            this.btnProxyCmd.Size = new System.Drawing.Size(238, 55);
            this.btnProxyCmd.TabIndex = 1;
            this.btnProxyCmd.Text = "ProxyCmd";
            this.btnProxyCmd.UseVisualStyleBackColor = true;
            this.btnProxyCmd.Click += new System.EventHandler(this.btnProxyCmd_Click);
            // 
            // btnWebcam
            // 
            this.btnWebcam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWebcam.Location = new System.Drawing.Point(13, 207);
            this.btnWebcam.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnWebcam.Name = "btnWebcam";
            this.btnWebcam.Size = new System.Drawing.Size(238, 55);
            this.btnWebcam.TabIndex = 2;
            this.btnWebcam.Text = "Webcam";
            this.btnWebcam.UseVisualStyleBackColor = true;
            this.btnWebcam.Click += new System.EventHandler(this.btnWebcam_Click);
            // 
            // btnTunnelTCP
            // 
            this.btnTunnelTCP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTunnelTCP.Location = new System.Drawing.Point(13, 270);
            this.btnTunnelTCP.Name = "btnTunnelTCP";
            this.btnTunnelTCP.Size = new System.Drawing.Size(238, 55);
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
            this.lblToolbox.Location = new System.Drawing.Point(12, 9);
            this.lblToolbox.Name = "lblToolbox";
            this.lblToolbox.Size = new System.Drawing.Size(239, 66);
            this.lblToolbox.TabIndex = 6;
            this.lblToolbox.Text = "Toolbox";
            this.lblToolbox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlSuspension1
            // 
            this.pnlSuspension1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSuspension1.Location = new System.Drawing.Point(18, 505);
            this.pnlSuspension1.Name = "pnlSuspension1";
            this.pnlSuspension1.Size = new System.Drawing.Size(232, 175);
            this.pnlSuspension1.TabIndex = 4;
            // 
            // pnlCover1
            // 
            this.pnlCover1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCover1.Location = new System.Drawing.Point(18, 351);
            this.pnlCover1.Name = "pnlCover1";
            this.pnlCover1.Size = new System.Drawing.Size(232, 148);
            this.pnlCover1.TabIndex = 3;
            // 
            // FrmToolbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 692);
            this.Controls.Add(this.lblToolbox);
            this.Controls.Add(this.btnTunnelTCP);
            this.Controls.Add(this.pnlSuspension1);
            this.Controls.Add(this.pnlCover1);
            this.Controls.Add(this.btnWebcam);
            this.Controls.Add(this.btnProxyCmd);
            this.Controls.Add(this.btnBase64);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
    }
}

