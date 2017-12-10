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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmToolbox));
            this.btnBase64 = new System.Windows.Forms.Button();
            this.btnProxyCmd = new System.Windows.Forms.Button();
            this.btnWebcam = new System.Windows.Forms.Button();
            this.pnlSuspension1 = new VAR.Toolbox.UI.PnlSuspension();
            this.pnlCover1 = new VAR.Toolbox.UI.PnlCover();
            this.SuspendLayout();
            // 
            // btnBase64
            // 
            this.btnBase64.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBase64.Location = new System.Drawing.Point(18, 18);
            this.btnBase64.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBase64.Name = "btnBase64";
            this.btnBase64.Size = new System.Drawing.Size(278, 52);
            this.btnBase64.TabIndex = 0;
            this.btnBase64.Text = "Base64";
            this.btnBase64.UseVisualStyleBackColor = true;
            this.btnBase64.Click += new System.EventHandler(this.btnBase64_Click);
            // 
            // btnProxyCmd
            // 
            this.btnProxyCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProxyCmd.Location = new System.Drawing.Point(18, 80);
            this.btnProxyCmd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnProxyCmd.Name = "btnProxyCmd";
            this.btnProxyCmd.Size = new System.Drawing.Size(278, 55);
            this.btnProxyCmd.TabIndex = 1;
            this.btnProxyCmd.Text = "ProxyCmd";
            this.btnProxyCmd.UseVisualStyleBackColor = true;
            this.btnProxyCmd.Click += new System.EventHandler(this.btnProxyCmd_Click);
            // 
            // btnWebcam
            // 
            this.btnWebcam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWebcam.Location = new System.Drawing.Point(18, 145);
            this.btnWebcam.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnWebcam.Name = "btnWebcam";
            this.btnWebcam.Size = new System.Drawing.Size(278, 55);
            this.btnWebcam.TabIndex = 2;
            this.btnWebcam.Text = "Webcam";
            this.btnWebcam.UseVisualStyleBackColor = true;
            this.btnWebcam.Click += new System.EventHandler(this.btnWebcam_Click);
            // 
            // pnlSuspension1
            // 
            this.pnlSuspension1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSuspension1.Location = new System.Drawing.Point(18, 373);
            this.pnlSuspension1.Name = "pnlSuspension1";
            this.pnlSuspension1.Size = new System.Drawing.Size(278, 175);
            this.pnlSuspension1.TabIndex = 4;
            // 
            // pnlCover1
            // 
            this.pnlCover1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCover1.Location = new System.Drawing.Point(18, 219);
            this.pnlCover1.Name = "pnlCover1";
            this.pnlCover1.Size = new System.Drawing.Size(278, 148);
            this.pnlCover1.TabIndex = 3;
            // 
            // FrmToolbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 560);
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
    }
}

